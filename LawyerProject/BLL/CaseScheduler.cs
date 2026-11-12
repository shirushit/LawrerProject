using System;
using System.Collections.Generic;
using System.Linq;

namespace LawyerProject.BLL
{
    /// <summary>A fixed step defined for a case type (e.g. "filing the claim", 14 days).</summary>
    public class StepTemplate
    {
        public int ProcessId { get; set; }
        public int Level { get; set; }
        public int DurationDays { get; set; }
    }

    /// <summary>A step that belongs to a specific case.</summary>
    public class ScheduledStep
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public int DurationDays { get; set; }
        public DateTime EstimatedDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsDone { get; set; }
    }

    /// <summary>Israeli work week: Sunday to Thursday.</summary>
    public static class BusinessCalendar
    {
        /// <summary>The same day, or the next Sunday when the date falls on Friday or Saturday.</summary>
        public static DateTime NextWorkday(DateTime date)
        {
            DateTime d = date.Date;
            if (d.DayOfWeek == DayOfWeek.Friday) return d.AddDays(2);
            if (d.DayOfWeek == DayOfWeek.Saturday) return d.AddDays(1);
            return d;
        }

        public static bool IsWorkday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday;
        }
    }

    /// <summary>
    /// The heart of QUEDIN: builds a case's timeline from its case type and keeps
    /// every later deadline up to date when a step is done, moved or undone.
    /// Estimated dates never fall on Friday or Saturday.
    /// Pure logic with no database access, so it is fully unit-tested.
    /// </summary>
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
                    ProcessId = t.ProcessId,
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
            ScheduledStep previous = ordered.Take(index).LastOrDefault();
            DateTime earliest = previous == null ? DateTime.MinValue : (previous.DoneDate ?? previous.EstimatedDate);
            if (newDate.Date < earliest.Date)
                throw new InvalidOperationException("המועד החדש מוקדם מהשלב הקודם (" + earliest.ToString("dd/MM/yyyy") + ").");

            step.EstimatedDate = newDate.Date;
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

        /// <summary>Open steps whose estimated date is within <paramref name="days"/> days (or already late).</summary>
        public static List<ScheduledStep> DueSoon(IEnumerable<ScheduledStep> steps, DateTime today, int days)
        {
            return Ordered(steps)
                .Where(s => !s.IsDone && s.EstimatedDate.Date <= today.Date.AddDays(days))
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
                cursor = BusinessCalendar.NextWorkday(cursor.AddDays(ordered[i].DurationDays));
                ordered[i].EstimatedDate = cursor;
            }
        }
    }
}
