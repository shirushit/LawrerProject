using System;
using System.Collections.Generic;
using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Connects the pure <see cref="CaseScheduler"/> and <see cref="FeeCalculator"/>
    /// to the Entity Framework model.
    /// </summary>
    public class CaseService
    {
        private readonly LawyerDBEntities1 db;

        public CaseService(LawyerDBEntities1 db)
        {
            this.db = db;
        }

        /// <summary>
        /// Adds the fixed steps of the case type to a new case and sets its status to the first step.
        /// Returns null on success or an error message.
        /// </summary>
        public string CreatePlan(Tikim_Letipul_Orech_Din tik, DateTime openedOn)
        {
            List<StepTemplate> templates = db.Taalichim_Kvuim_Batik
                .Where(k => k.IDSubjektTik == tik.IDSubjektTik)
                .ToList()
                .Select(k => new StepTemplate
                {
                    ProcessId = k.IDTaalich,
                    Level = k.NumLevel,
                    DurationDays = ParseDays(k.Taalichim_Letikim.MeshechZmanLetaalich)
                })
                .ToList();

            if (templates.Count == 0)
                return "לא הוגדרו תהליכים קבועים לנושא התיק שנבחר. יש להגדיר אותם במסך נושאי תיקים.";

            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, openedOn);
            foreach (ScheduledStep step in plan)
            {
                tik.Tahalich_Tipul_Betik.Add(new Tahalich_Tipul_Betik
                {
                    IDTahalich = step.ProcessId,
                    TaharichMeshoar = step.EstimatedDate,
                    TaharichBizhua = null,
                    Buzha = false
                });
            }
            tik.StatusTik = plan[0].ProcessId;
            return null;
        }

        /// <summary>
        /// Marks a step of the case as done and pushes the dates of the following steps.
        /// Returns null on success or an error message (for example when a step is skipped).
        /// </summary>
        public string CompleteStep(Tikim_Letipul_Orech_Din tik, int stepId, DateTime doneOn)
        {
            List<Tahalich_Tipul_Betik> entities = tik.Tahalich_Tipul_Betik.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.IDTahalichTipulLetik, ToScheduled);

            ScheduledStep target;
            if (!map.TryGetValue(stepId, out target)) return "התהליך שנבחר אינו שייך לתיק.";

            try
            {
                CaseScheduler.CompleteStep(map.Values.ToList(), target, doneOn);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }

            foreach (Tahalich_Tipul_Betik e in entities)
            {
                ScheduledStep s = map[e.IDTahalichTipulLetik];
                e.Buzha = s.IsDone;
                e.TaharichBizhua = s.DoneDate;
                e.TaharichMeshoar = s.EstimatedDate;
            }

            ScheduledStep next = CaseScheduler.NextPending(map.Values);
            if (next != null) tik.StatusTik = next.ProcessId;

            db.SaveChanges();
            return null;
        }

        /// <summary>Steps of the case in timeline order.</summary>
        public static List<Tahalich_Tipul_Betik> OrderedSteps(Tikim_Letipul_Orech_Din tik)
        {
            return tik.Tahalich_Tipul_Betik
                .OrderBy(s => s.TaharichMeshoar ?? DateTime.MaxValue)
                .ThenBy(s => s.IDTahalichTipulLetik)
                .ToList();
        }

        /// <summary>True when every step of the case is done.</summary>
        public static bool IsFinished(Tikim_Letipul_Orech_Din tik)
        {
            return tik.Tahalich_Tipul_Betik.Count > 0 && tik.Tahalich_Tipul_Betik.All(s => s.Buzha == true);
        }

        /// <summary>What the client still owes: the last payment's balance, or the full case total.</summary>
        public static decimal OpenBalance(Tikim_Letipul_Orech_Din tik)
        {
            Tashlumim_Letik last = tik.Tashlumim_Letik.OrderByDescending(p => p.IDTashlum).FirstOrDefault();
            if (last != null) return last.Itra;

            decimal courtFee = tik.Subjekt_Tik != null ? (tik.Subjekt_Tik.Agra ?? 0m) : 0m;
            return FeeCalculator.CaseTotal(tik.SchumLetvia ?? 0m, tik.AchuzRevachim ?? 0d, courtFee);
        }

        public static int ParseDays(string value)
        {
            int days;
            return int.TryParse(value, out days) && days > 0 ? days : 0;
        }

        private static ScheduledStep ToScheduled(Tahalich_Tipul_Betik e)
        {
            return new ScheduledStep
            {
                Id = e.IDTahalichTipulLetik,
                ProcessId = e.IDTahalich,
                DurationDays = e.Taalichim_Letikim != null ? ParseDays(e.Taalichim_Letikim.MeshechZmanLetaalich) : 0,
                EstimatedDate = e.TaharichMeshoar ?? DateTime.MaxValue.Date,
                DoneDate = e.TaharichBizhua,
                IsDone = e.Buzha == true
            };
        }
    }
}
