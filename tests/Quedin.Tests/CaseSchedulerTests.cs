using System;
using System.Collections.Generic;
using System.Linq;
using LawyerProject.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.Tests
{
    [TestClass]
    public class CaseSchedulerTests
    {
        // A Monday; with 7/14/21-day steps every estimate is also a Monday, so no weekend shift applies.
        private static readonly DateTime Opened = new DateTime(2024, 3, 4);

        private static List<StepTemplate> CivilClaim()
        {
            // Deliberately out of order: the plan must follow Level, not list order.
            return new List<StepTemplate>
            {
                new StepTemplate { ProcessId = 30, Level = 3, DurationDays = 21 },
                new StepTemplate { ProcessId = 10, Level = 1, DurationDays = 7 },
                new StepTemplate { ProcessId = 20, Level = 2, DurationDays = 14 },
            };
        }

        private static List<ScheduledStep> WithIds(List<ScheduledStep> plan)
        {
            for (int i = 0; i < plan.Count; i++) plan[i].Id = i + 1;
            return plan;
        }

        [TestMethod]
        public void BuildPlan_OrdersByLevel_AndAccumulatesDurations()
        {
            List<ScheduledStep> plan = CaseScheduler.BuildPlan(CivilClaim(), Opened);

            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, plan.Select(s => s.ProcessId).ToArray());
            Assert.AreEqual(Opened.AddDays(7), plan[0].EstimatedDate);
            Assert.AreEqual(Opened.AddDays(21), plan[1].EstimatedDate);
            Assert.AreEqual(Opened.AddDays(42), plan[2].EstimatedDate);
            Assert.IsTrue(plan.All(s => !s.IsDone && s.DoneDate == null));
        }

        [TestMethod]
        public void BuildPlan_IgnoresTimeOfDay_AndNegativeDurations()
        {
            var templates = new List<StepTemplate> { new StepTemplate { ProcessId = 1, Level = 1, DurationDays = -5 } };
            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, Opened.AddHours(15));

            Assert.AreEqual(Opened, plan[0].EstimatedDate);
            Assert.AreEqual(0, plan[0].DurationDays);
        }

        [TestMethod]
        public void BuildPlan_EmptyTemplates_ReturnsEmptyPlan()
        {
            Assert.AreEqual(0, CaseScheduler.BuildPlan(new List<StepTemplate>(), Opened).Count);
        }

        [TestMethod]
        public void CompleteStep_Late_PushesAllFollowingDeadlines()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            DateTime doneLate = Opened.AddDays(10); // planned for day 7; a Thursday

            CaseScheduler.CompleteStep(plan, plan[0], doneLate);

            Assert.IsTrue(plan[0].IsDone);
            Assert.AreEqual(doneLate, plan[0].DoneDate);
            Assert.AreEqual(doneLate.AddDays(14), plan[1].EstimatedDate);
            Assert.AreEqual(doneLate.AddDays(14 + 21), plan[2].EstimatedDate);
        }

        [TestMethod]
        public void CompleteStep_Early_PullsDeadlinesForward()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            DateTime doneEarly = Opened.AddDays(2);

            CaseScheduler.CompleteStep(plan, plan[0], doneEarly);

            Assert.AreEqual(doneEarly.AddDays(14), plan[1].EstimatedDate);
        }

        [TestMethod]
        public void CompleteStep_SkippingAStep_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));

            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.CompleteStep(plan, plan[1], Opened));
            Assert.IsFalse(plan[1].IsDone);
        }

        [TestMethod]
        public void CompleteStep_Twice_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened);

            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.CompleteStep(plan, plan[0], Opened));
        }

        [TestMethod]
        public void CompleteStep_InSequence_FinishesCase()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));

            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(7));
            Assert.AreSame(plan[1], CaseScheduler.NextPending(plan));
            CaseScheduler.CompleteStep(plan, plan[1], Opened.AddDays(20));
            CaseScheduler.CompleteStep(plan, plan[2], Opened.AddDays(40));

            Assert.IsNull(CaseScheduler.NextPending(plan));
        }

        [TestMethod]
        public void DueSoon_ReturnsLateAndUpcomingOpenSteps()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            DateTime today = Opened.AddDays(18); // step 1 late (day 7), step 2 due day 21, step 3 day 42

            List<ScheduledStep> due = CaseScheduler.DueSoon(plan, today, 7);

            CollectionAssert.AreEqual(new[] { 10, 20 }, due.Select(s => s.ProcessId).ToArray());
        }

        // ---------------------------------------------------------- workdays

        [TestMethod]
        public void BusinessCalendar_FridayAndSaturday_MoveToSunday()
        {
            Assert.AreEqual(new DateTime(2024, 3, 3), BusinessCalendar.NextWorkday(new DateTime(2024, 3, 1)));  // Fri
            Assert.AreEqual(new DateTime(2024, 3, 3), BusinessCalendar.NextWorkday(new DateTime(2024, 3, 2)));  // Sat
            Assert.AreEqual(new DateTime(2024, 3, 7), BusinessCalendar.NextWorkday(new DateTime(2024, 3, 7)));  // Thu stays
            Assert.IsFalse(BusinessCalendar.IsWorkday(new DateTime(2024, 3, 2)));
            Assert.IsTrue(BusinessCalendar.IsWorkday(new DateTime(2024, 3, 3)));
        }

        [TestMethod]
        public void BuildPlan_DeadlineOnWeekend_MovesToSunday_AndNextStepCountsFromThere()
        {
            var templates = new List<StepTemplate>
            {
                new StepTemplate { ProcessId = 1, Level = 1, DurationDays = 4 }, // Mon + 4 = Fri -> Sun 10/3
                new StepTemplate { ProcessId = 2, Level = 2, DurationDays = 6 }, // Sun + 6 = Sat -> Sun 17/3
            };
            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, Opened);

            Assert.AreEqual(new DateTime(2024, 3, 10), plan[0].EstimatedDate);
            Assert.AreEqual(new DateTime(2024, 3, 17), plan[1].EstimatedDate);
            Assert.IsTrue(plan.All(s => BusinessCalendar.IsWorkday(s.EstimatedDate)));
        }

        [TestMethod]
        public void CompleteStep_FollowingDeadlinesNeverFallOnWeekend()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], new DateTime(2024, 3, 9)); // Sat + 14 = Sat -> Sun

            Assert.AreEqual(new DateTime(2024, 3, 24), plan[1].EstimatedDate);
            Assert.IsTrue(BusinessCalendar.IsWorkday(plan[2].EstimatedDate));
        }

        // ---------------------------------------------------------- completion date rules

        [TestMethod]
        public void CompleteStep_DateBeforePreviousStepDone_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(10));

            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.CompleteStep(plan, plan[1], Opened.AddDays(9)));
            Assert.IsFalse(plan[1].IsDone);
        }

        [TestMethod]
        public void CompleteStep_SameDayAsPreviousStep_IsAllowed()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(3));
            CaseScheduler.CompleteStep(plan, plan[1], Opened.AddDays(3));

            Assert.IsTrue(plan[1].IsDone);
        }

        // ---------------------------------------------------------- order

        [TestMethod]
        public void Ordered_FollowsCreationOrder_NotDates()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            plan[2].EstimatedDate = Opened; // a strange date must not move step 3 to the front

            Assert.AreSame(plan[0], CaseScheduler.NextPending(plan));
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, CaseScheduler.Ordered(plan).Select(s => s.Id).ToArray());
        }

        // ---------------------------------------------------------- reschedule

        [TestMethod]
        public void Reschedule_KeepsExactDate_AndPushesFollowingSteps()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            DateTime hearing = new DateTime(2024, 5, 2); // Thursday, set by the court

            CaseScheduler.Reschedule(plan, plan[1], hearing);

            Assert.AreEqual(hearing, plan[1].EstimatedDate);
            Assert.AreEqual(hearing.AddDays(21), plan[2].EstimatedDate);
            Assert.AreEqual(Opened.AddDays(7), plan[0].EstimatedDate, "earlier steps are not touched");
        }

        [TestMethod]
        public void Reschedule_CanMoveADeadlineEarlier()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.Reschedule(plan, plan[1], Opened.AddDays(10));

            Assert.AreEqual(Opened.AddDays(10), plan[1].EstimatedDate);
            Assert.AreEqual(Opened.AddDays(31), plan[2].EstimatedDate);
        }

        [TestMethod]
        public void Reschedule_DoneStep_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(7));

            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.Reschedule(plan, plan[0], Opened.AddDays(20)));
        }

        [TestMethod]
        public void Reschedule_BeforePreviousStep_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(8));

            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.Reschedule(plan, plan[1], Opened.AddDays(7)));
            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.Reschedule(plan, plan[2], Opened));
        }

        // ---------------------------------------------------------- undo

        [TestMethod]
        public void UndoLastCompletion_ReopensStep_AndRecalculatesFromItsEstimate()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(10));

            ScheduledStep undone = CaseScheduler.UndoLastCompletion(plan);

            Assert.AreSame(plan[0], undone);
            Assert.IsFalse(plan[0].IsDone);
            Assert.IsNull(plan[0].DoneDate);
            Assert.AreEqual(Opened.AddDays(21), plan[1].EstimatedDate);
            Assert.AreSame(plan[0], CaseScheduler.NextPending(plan));
        }

        [TestMethod]
        public void UndoLastCompletion_OnlyUndoesTheLatestStep()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            CaseScheduler.CompleteStep(plan, plan[0], Opened.AddDays(7));
            CaseScheduler.CompleteStep(plan, plan[1], Opened.AddDays(21));

            CaseScheduler.UndoLastCompletion(plan);

            Assert.IsTrue(plan[0].IsDone);
            Assert.IsFalse(plan[1].IsDone);
        }

        [TestMethod]
        public void UndoLastCompletion_NothingDone_Throws()
        {
            List<ScheduledStep> plan = WithIds(CaseScheduler.BuildPlan(CivilClaim(), Opened));
            Assert.ThrowsException<InvalidOperationException>(() => CaseScheduler.UndoLastCompletion(plan));
        }

        [TestMethod]
        public void DaysLate_PositiveWhenLate_NegativeWhenAhead()
        {
            Assert.AreEqual(3, CaseScheduler.DaysLate(Opened, Opened.AddDays(3)));
            Assert.AreEqual(-2, CaseScheduler.DaysLate(Opened.AddDays(2), Opened));
            Assert.AreEqual(0, CaseScheduler.DaysLate(Opened, Opened.AddHours(20)));
        }
    }
}
