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
        private static readonly DateTime Opened = new DateTime(2024, 3, 1);

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
            DateTime doneLate = Opened.AddDays(12); // planned for day 7

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
    }
}
