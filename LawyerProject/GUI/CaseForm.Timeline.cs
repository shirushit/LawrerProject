using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>CaseForm: the steps tab (timeline of the case).</summary>
    public partial class CaseForm
    {
        private void FillSteps()
        {
            listSteps.Items.Clear();
            DateTime today = Clock.Today;
            CaseStep next = CaseService.OrderedSteps(legalCase).FirstOrDefault(s => s.IsDone != true);
            int number = 1;
            foreach (CaseStep step in CaseService.OrderedSteps(legalCase))
            {
                bool done = step.IsDone == true;
                bool late = CaseScheduler.UrgencyOf(step.DueDate, done, today, 0) == Urgency.Late;
                string status = done ? "בוצע" : late ? "באיחור" : (step == next ? "השלב הבא" : "ממתין");
                if (!done && step.IsFixedDate) status += " · מועד קבוע";
                string lateText = "";
                if (late) lateText = CaseScheduler.DaysLate(step.DueDate.Value, today) + " ימים";
                else if (done && step.DoneDate.HasValue && step.DueDate.HasValue && step.DoneDate.Value > step.DueDate.Value)
                    lateText = "בוצע באיחור";

                var item = new ListViewItem(new[]
                {
                    number.ToString(),
                    step.Procedure != null ? step.Procedure.Name : step.ProcedureId.ToString(),
                    Ui.FormatDate(step.DueDate),
                    done ? Ui.FormatDate(step.DoneDate) : "",
                    status,
                    lateText,
                    step.Notes ?? ""
                });
                item.Tag = step.CaseStepId;
                if (done) item.ForeColor = SystemColors.GrayText;
                else if (late) item.BackColor = Color.MistyRose;
                if (step == next) item.Font = new Font(listSteps.Font, FontStyle.Bold);
                listSteps.Items.Add(item);
                number++;
            }
            btnCompleteStep.Enabled = next != null;
            btnReschedule.Enabled = next != null;
            btnStepNote.Enabled = listSteps.Items.Count > 0;
            btnUndoStep.Visible = Session.IsManager;
            btnUndoStep.Enabled = legalCase.Steps.Any(st => st.IsDone == true);
        }


        private CaseStep NextStep()
        {
            return CaseService.OrderedSteps(legalCase).FirstOrDefault(s => s.IsDone != true);
        }

        private static string StepName(CaseStep step)
        {
            return step.Procedure != null ? step.Procedure.Name : "";
        }

        private CaseStep SelectedStep()
        {
            if (listSteps.SelectedItems.Count == 0) return null;
            int id = (int)listSteps.SelectedItems[0].Tag;
            return legalCase.Steps.FirstOrDefault(s => s.CaseStepId == id);
        }

        private void btnCompleteStep_Click(object sender, EventArgs e)
        {
            CaseStep next = NextStep();
            if (next == null) return;

            CaseStep previous = CaseService.OrderedSteps(legalCase).LastOrDefault(s => s.IsDone == true);
            DateTime? min = previous != null ? previous.DoneDate : null;
            DateTime doneOn;
            string unused;
            if (!Ui.AskDate(this, "סימון שלב כבוצע",
                    "באיזה תאריך בוצע השלב \"" + StepName(next) + "\"?\nהמועדים של כל השלבים הבאים יחושבו מתאריך זה.",
                    Clock.Today, min, Clock.Today, null, null, out doneOn, out unused)) return;

            RunOperation(svc => svc.CompleteStep(legalCase, next.CaseStepId, doneOn),
                "השלב \"" + StepName(next) + "\" סומן כבוצע.", tabSteps);
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            CaseStep step = SelectedStep();
            if (step == null || step.IsDone == true) step = NextStep();
            if (step == null) return;

            DateTime newDate;
            string note;
            if (!Ui.AskDate(this, "שינוי מועד",
                    "מועד חדש לשלב \"" + StepName(step) + "\" (למשל מועד דיון שנקבע ע\"י בית המשפט).\nהמועד יסומן כקבוע: החישוב האוטומטי לא יזיז אותו, והשלבים הבאים יחושבו ממנו.",
                    step.DueDate ?? Clock.Today, Clock.Today.AddYears(-CaseService.MaxYearsAway), Clock.Today.AddYears(CaseService.MaxYearsAway), "סיבת השינוי (לא חובה)", step.Notes, out newDate, out note)) return;

            if (!BusinessCalendar.IsWorkday(newDate))
            {
                string holiday = BusinessCalendar.HolidayName(newDate);
                if (!Ui.Confirm("המועד שנבחר הוא " + (holiday ?? Ui.DayName(newDate)) + ". לשמור בכל זאת?")) return;
            }

            RunOperation(svc => svc.RescheduleStep(legalCase, step.CaseStepId, newDate, note),
                "המועד עודכן ל-" + newDate.ToString("dd/MM/yyyy") + ".", tabSteps);
        }

        private void btnStepNote_Click(object sender, EventArgs e)
        {
            CaseStep step = SelectedStep() ?? NextStep();
            if (step == null) return;
            string note = Ui.AskText(this, "הערה לשלב", "הערה לשלב \"" + StepName(step) + "\" (עד 200 תווים):", step.Notes, 200);
            if (note == null) return;
            RunOperation(svc => svc.SetNote(legalCase, step.CaseStepId, note), "ההערה נשמרה.", tabSteps);
        }

        private void btnUndoStep_Click(object sender, EventArgs e)
        {
            CaseStep last = CaseService.OrderedSteps(legalCase).LastOrDefault(s => s.IsDone == true);
            if (last == null) return;
            if (!Ui.Confirm("לבטל את סימון השלב \"" + StepName(last) + "\" כבוצע?\nהשלב יחזור להיות השלב הבא, ומועדי השלבים שאחריו יחושבו מחדש.")) return;
            RunOperation(svc => svc.UndoLastStep(legalCase), "סימון השלב בוטל.", tabSteps);
        }
    }
}
