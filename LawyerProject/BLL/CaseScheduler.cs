using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LawyerProject.BLL
{
    /// <summary>A fixed step defined for a case type (e.g. "filing the claim", 14 days).</summary>
    public class StepTemplate
    {
        public int ProcedureId { get; set; }
        public int Level { get; set; }
        public int DurationDays { get; set; }
    }

    /// <summary>A step that belongs to a specific case.</summary>
    public class ScheduledStep
    {
        public int Id { get; set; }
        public int ProcedureId { get; set; }
        public int DurationDays { get; set; }
        public DateTime EstimatedDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsDone { get; set; }

        /// <summary>
        /// A date decided outside the office (for example a hearing set by the court).
        /// Automatic recalculation never moves it; later steps are counted from it.
        /// </summary>
        public bool IsFixed { get; set; }
    }

    /// <summary>
    /// Israeli work days: Sunday to Thursday, excluding the Jewish holidays on which courts are closed
    /// (computed from the Hebrew calendar) and any extra days the office adds (for example Independence
    /// Day or a court recess).
    /// </summary>
    public static class BusinessCalendar
    {
        private static readonly HebrewCalendar Hebrew = new HebrewCalendar();
        private static readonly Dictionary<int, Dictionary<DateTime, string>> ByYear = new Dictionary<int, Dictionary<DateTime, string>>();
        private static Dictionary<DateTime, string> extra = new Dictionary<DateTime, string>();

        /// <summary>Replaces the office's own non-working days (loaded from the Holidays table).</summary>
        public static void SetExtraHolidays(IDictionary<DateTime, string> days)
        {
            extra = days == null
                ? new Dictionary<DateTime, string>()
                : days.ToDictionary(kv => kv.Key.Date, kv => kv.Value);
        }

        /// <summary>The holiday's name, or null when the date is not a holiday.</summary>
        public static string HolidayName(DateTime date)
        {
            string name;
            DateTime d = date.Date;
            if (extra.TryGetValue(d, out name)) return name;
            return JewishHolidays(d.Year).TryGetValue(d, out name) ? name : null;
        }

        /// <summary>
        /// Holidays on which Israeli courts do not sit, for one Gregorian year: Rosh Hashana (2 days),
        /// Yom Kippur, Sukkot, Shemini Atzeret, the first and seventh days of Pesach and Shavuot.
        /// </summary>
        public static Dictionary<DateTime, string> JewishHolidays(int year)
        {
            Dictionary<DateTime, string> days;
            if (ByYear.TryGetValue(year, out days)) return days;

            days = new Dictionary<DateTime, string>();
            int first = Hebrew.GetYear(new DateTime(year, 1, 1));
            for (int hy = first; hy <= first + 1; hy++)
            {
                int nisan = Hebrew.IsLeapYear(hy) ? 8 : 7;
                int sivan = Hebrew.IsLeapYear(hy) ? 10 : 9;
                var list = new[]
                {
                    Tuple.Create(1, 1, "ראש השנה"), Tuple.Create(1, 2, "ראש השנה"), Tuple.Create(1, 10, "יום כיפור"),
                    Tuple.Create(1, 15, "סוכות"), Tuple.Create(1, 22, "שמיני עצרת"),
                    Tuple.Create(nisan, 15, "פסח"), Tuple.Create(nisan, 21, "שביעי של פסח"), Tuple.Create(sivan, 6, "שבועות")
                };
                foreach (var h in list)
                {
                    DateTime d = Hebrew.ToDateTime(hy, h.Item1, h.Item2, 0, 0, 0, 0);
                    if (d.Year == year) days[d] = h.Item3;
                }
            }
            ByYear[year] = days;
            return days;
        }

        public static bool IsWorkday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday && HolidayName(date) == null;
        }

        /// <summary>The same day, or the next work day when the date falls on a weekend or a holiday.</summary>
        public static DateTime NextWorkday(DateTime date)
        {
            DateTime d = date.Date;
            while (!IsWorkday(d)) d = d.AddDays(1);
            return d;
        }
    }

    /// <summary>
    /// The heart of QUEDIN: builds a case's timeline from its case type and keeps
    /// every later deadline up to date when a step is done, moved or undone.
    /// Estimated dates never fall on a weekend or a holiday, and dates fixed by the court are never moved.
    /// Pure logic with no database access, so it is fully unit-tested.
    /// </summary>
    /// <summary>How urgent an open step is.</summary>
    public enum Urgency
    {
        None,
        DueSoon,
        Late
    }

    public static class CaseScheduler
    {
        /// <summary>
        /// Creates the timeline of a new case. Each step is due DurationDays after the
        /// previous one, starting from <paramref name="openedOn"/>, moved to a workday.
        /// </summary>
        public static List<ScheduledStep> BuildPlan(IEnumerable<StepTemplate> templates, DateTime openedOn)
        {
            if (templates == null) throw new ArgumentNullException(nameof(templates));

            var plan = new List<ScheduledStep>();
            DateTime cursor = openedOn.Date;
            foreach (StepTemplate t in templates.OrderBy(x => x.Level))
            {
                int days = Math.Max(0, t.DurationDays);
                cursor = BusinessCalendar.NextWorkday(cursor.AddDays(days));
                plan.Add(new ScheduledStep
                {
                    ProcedureId = t.ProcedureId,
                    DurationDays = days,
                    EstimatedDate = cursor,
                    IsDone = false
                });
            }
            return plan;
        }

        /// <summary>
        /// Steps in timeline order = the order they were created in (the case type's levels).
        /// Not by date: a court-set date may legitimately be earlier than a previous step's estimate.
        /// </summary>
        public static List<ScheduledStep> Ordered(IEnumerable<ScheduledStep> steps)
        {
            return steps.OrderBy(s => s.Id).ToList(); // OrderBy is stable, so a new plan (all ids 0) keeps its order
        }

        /// <summary>The first step that has not been completed yet, or null when the case is finished.</summary>
        public static ScheduledStep NextPending(IEnumerable<ScheduledStep> steps)
        {
            return Ordered(steps).FirstOrDefault(s => !s.IsDone);
        }

        /// <summary>The most recently completed step, or null.</summary>
        public static ScheduledStep LastDone(IEnumerable<ScheduledStep> steps)
        {
            return Ordered(steps).LastOrDefault(s => s.IsDone);
        }

        /// <summary>
        /// Marks a step as done on <paramref name="doneOn"/> and recalculates the estimated dates of
        /// all following open steps from that date. Only the next pending step may be completed,
        /// and not before the previous step was completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">The step is skipped, already done, or the date is earlier than the previous step.</exception>
        public static void CompleteStep(IList<ScheduledStep> steps, ScheduledStep step, DateTime doneOn)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (step.IsDone) throw new InvalidOperationException("השלב כבר סומן כבוצע.");

            List<ScheduledStep> ordered = Ordered(steps);
            ScheduledStep next = ordered.FirstOrDefault(s => !s.IsDone);
            if (!ReferenceEquals(next, step))
                throw new InvalidOperationException("לא ניתן לדלג על שלבים: יש להשלים קודם את השלב הקודם.");

            ScheduledStep previous = ordered.LastOrDefault(s => s.IsDone);
            if (previous != null && previous.DoneDate.HasValue && doneOn.Date < previous.DoneDate.Value.Date)
                throw new InvalidOperationException("תאריך הביצוע מוקדם מתאריך הביצוע של השלב הקודם (" +
                                                    previous.DoneDate.Value.ToString("dd/MM/yyyy") + ").");

            step.IsDone = true;
            step.DoneDate = doneOn.Date;
            PushFollowing(ordered, ordered.IndexOf(step), doneOn.Date);
        }

        /// <summary>
        /// Sets an open step to a date decided outside the office (for example a hearing date set by
        /// the court) and recalculates every later open step from it. The date is kept exactly as given.
        /// </summary>
        public static void Reschedule(IList<ScheduledStep> steps, ScheduledStep step, DateTime newDate)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (step.IsDone) throw new InvalidOperationException("לא ניתן לשנות מועד של שלב שכבר בוצע.");

            List<ScheduledStep> ordered = Ordered(steps);
            int index = ordered.IndexOf(step);
            // Only a step that was really completed limits the new date. An estimate of an open
            // earlier step does not: the court may set a hearing before the office's estimate.
            ScheduledStep lastDone = ordered.Take(index).LastOrDefault(s => s.IsDone && s.DoneDate.HasValue);
            if (lastDone != null && newDate.Date < lastDone.DoneDate.Value.Date)
                throw new InvalidOperationException("המועד החדש מוקדם מתאריך הביצוע של השלב הקודם (" +
                                                    lastDone.DoneDate.Value.ToString("dd/MM/yyyy") + ").");

            step.EstimatedDate = newDate.Date;
            step.IsFixed = true;
            PushFollowing(ordered, index, newDate.Date);
        }

        /// <summary>
        /// Undoes the most recent completion: the step is open again and the following open steps
        /// are recalculated from its estimated date.
        /// </summary>
        public static ScheduledStep UndoLastCompletion(IList<ScheduledStep> steps)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            List<ScheduledStep> ordered = Ordered(steps);
            ScheduledStep last = ordered.LastOrDefault(s => s.IsDone);
            if (last == null) throw new InvalidOperationException("אין שלב שסומן כבוצע.");

            last.IsDone = false;
            last.DoneDate = null;
            PushFollowing(ordered, ordered.IndexOf(last), last.EstimatedDate);
            return last;
        }

        /// <summary>
        /// Open steps whose calculated date falls after a later step with a fixed date (for example,
        /// evidence is due after the hearing the court already set). The office must decide what to do,
        /// so these are reported, never silently changed.
        /// </summary>
        public static List<ScheduledStep> Conflicts(IEnumerable<ScheduledStep> steps)
        {
            List<ScheduledStep> open = Ordered(steps).Where(s => !s.IsDone).ToList();
            var conflicts = new List<ScheduledStep>();
            DateTime limit = DateTime.MaxValue;
            for (int i = open.Count - 1; i >= 0; i--)
            {
                if (open[i].EstimatedDate.Date > limit) conflicts.Insert(0, open[i]);
                if (open[i].IsFixed && open[i].EstimatedDate.Date < limit) limit = open[i].EstimatedDate.Date;
            }
            return conflicts;
        }

        /// <summary>
        /// The one rule for "late" and "due soon", used by the home screen, the cases list and the case
        /// screen: an open step is late when its date has passed, and due soon within <paramref name="soonDays"/> days.
        /// </summary>
        public static Urgency UrgencyOf(DateTime? due, bool isDone, DateTime today, int soonDays)
        {
            if (isDone || !due.HasValue) return Urgency.None;
            if (due.Value.Date < today.Date) return Urgency.Late;
            if (due.Value.Date <= today.Date.AddDays(soonDays)) return Urgency.DueSoon;
            return Urgency.None;
        }

        /// <summary>Open steps that are late or due within <paramref name="days"/> days.</summary>
        public static List<ScheduledStep> DueSoon(IEnumerable<ScheduledStep> steps, DateTime today, int days)
        {
            return Ordered(steps)
                .Where(s => UrgencyOf(s.EstimatedDate, s.IsDone, today, days) != Urgency.None)
                .ToList();
        }

        /// <summary>Days late (positive) or days left (negative or zero) for an estimated date.</summary>
        public static int DaysLate(DateTime estimated, DateTime today)
        {
            return (today.Date - estimated.Date).Days;
        }

        private static void PushFollowing(List<ScheduledStep> ordered, int index, DateTime from)
        {
            DateTime cursor = from;
            for (int i = index + 1; i < ordered.Count; i++)
            {
                if (ordered[i].IsDone) continue;
                if (ordered[i].IsFixed)
                {
                    cursor = ordered[i].EstimatedDate; // a court-set date is kept; later steps count from it
                    continue;
                }
                cursor = BusinessCalendar.NextWorkday(cursor.AddDays(ordered[i].DurationDays));
                ordered[i].EstimatedDate = cursor;
            }
        }
    }
}
