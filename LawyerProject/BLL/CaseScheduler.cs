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

    /// <summary>
    /// The heart of QUEDIN: builds a case's timeline from its case type and
    /// pushes every later deadline when a step is completed early or late.
    /// Pure logic with no database access, so it is fully unit-tested.
    /// </summary>
    public static class CaseScheduler
    {
        /// <summary>
        /// Creates the timeline of a new case. Each step is due
        /// DurationDays after the previous one, starting from <paramref name="openedOn"/>.
        /// </summary>
        public static List<ScheduledStep> BuildPlan(IEnumerable<StepTemplate> templates, DateTime openedOn)
        {
            if (templates == null) throw new ArgumentNullException(nameof(templates));

            var plan = new List<ScheduledStep>();
            DateTime cursor = openedOn.Date;
            foreach (StepTemplate t in templates.OrderBy(x => x.Level))
            {
                cursor = cursor.AddDays(Math.Max(0, t.DurationDays));
                plan.Add(new ScheduledStep
                {
                    ProcessId = t.ProcessId,
                    DurationDays = Math.Max(0, t.DurationDays),
                    EstimatedDate = cursor,
                    IsDone = false
                });
            }
            return plan;
        }

        /// <summary>Steps in timeline order.</summary>
        public static List<ScheduledStep> Ordered(IEnumerable<ScheduledStep> steps)
        {
            return steps.OrderBy(s => s.EstimatedDate).ThenBy(s => s.Id).ToList();
        }

        /// <summary>The first step that has not been completed yet, or null when the case is finished.</summary>
        public static ScheduledStep NextPending(IEnumerable<ScheduledStep> steps)
        {
            return Ordered(steps).FirstOrDefault(s => !s.IsDone);
        }

        /// <summary>
        /// Marks a step as done on <paramref name="doneOn"/> and recalculates the
        /// estimated dates of all following open steps from that date.
        /// Steps cannot be skipped: only the next pending step may be completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">The step is not the next pending step.</exception>
        public static void CompleteStep(IList<ScheduledStep> steps, ScheduledStep step, DateTime doneOn)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (step.IsDone) throw new InvalidOperationException("השלב כבר סומן כבוצע.");

            List<ScheduledStep> ordered = Ordered(steps);
            ScheduledStep next = ordered.FirstOrDefault(s => !s.IsDone);
            if (!ReferenceEquals(next, step))
                throw new InvalidOperationException("לא ניתן לדלג על שלבים: יש להשלים קודם את השלב הקודם.");

            int index = ordered.IndexOf(step);
            step.IsDone = true;
            step.DoneDate = doneOn.Date;

            DateTime cursor = doneOn.Date;
            for (int i = index + 1; i < ordered.Count; i++)
            {
                if (ordered[i].IsDone) continue;
                cursor = cursor.AddDays(ordered[i].DurationDays);
                ordered[i].EstimatedDate = cursor;
            }
        }

        /// <summary>Open steps whose estimated date is within <paramref name="days"/> days (or already late).</summary>
        public static List<ScheduledStep> DueSoon(IEnumerable<ScheduledStep> steps, DateTime today, int days)
        {
            return Ordered(steps)
                .Where(s => !s.IsDone && s.EstimatedDate.Date <= today.Date.AddDays(days))
                .ToList();
        }
    }
}
