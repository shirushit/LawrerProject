using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Windows.Forms;

namespace LawyerProject.GUI
{
    /// <summary>Shared helpers so every screen shows messages and grids the same way.</summary>
    internal static class Ui
    {
        private const string Title = "QUEDIN";
        private const MessageBoxOptions Rtl = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;

        public static void Info(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, Rtl);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, Rtl);
        }

        /// <summary>Shows a friendly message for database errors; technical details only as a second line.</summary>
        public static void Error(string message, Exception ex)
        {
            if (ex is DbUpdateConcurrencyException)
            {
                Error(ConcurrencyMessage);
                return;
            }
            Error(message + Environment.NewLine + FriendlyReason(ex));
        }

        public const string ConcurrencyMessage =
            "משתמש אחר עדכן את התיק בזמן שעבדת עליו, ולכן השינוי שלך לא נשמר.\nהתיק ייטען מחדש עם הנתונים העדכניים - יש לבצע את השינוי שוב.";

        /// <summary>Translates the common technical failures into words an office worker can act on.</summary>
        public static string FriendlyReason(Exception ex)
        {
            Exception root = ex.GetBaseException();
            var sql = root as System.Data.SqlClient.SqlException;
            if (sql != null)
            {
                switch (sql.Number)
                {
                    case -1:
                    case 2:
                    case 53:
                    case -2:
                        return "אין חיבור למסד הנתונים. ודאו שהשרת פועל ונסו שוב (פרטים למנהל המערכת: SQL " + sql.Number + ").";
                    case 547:
                        return "לא ניתן לבצע את הפעולה כי קיימים נתונים שתלויים ברשומה זו.";
                    case 2601:
                    case 2627:
                        return "קיימת כבר רשומה עם אותו מזהה.";
                    case 3201:
                    case 3013:
                        return "לא ניתן לכתוב לתיקיית הגיבוי. בחרו תיקייה ששרת מסד הנתונים מורשה לכתוב אליה.";
                }
            }
            if (root is System.Data.Entity.Validation.DbEntityValidationException || ex is System.Data.Entity.Validation.DbEntityValidationException)
                return "אחד השדות ארוך מדי או חסר.";
            return "פרטים טכניים: " + root.Message;
        }

        /// <summary>
        /// A short message at the bottom of the form that disappears by itself after a few seconds,
        /// for confirmations such as "saved" that do not need the user to click OK.
        /// </summary>
        public static void Notify(Form form, string message)
        {
            if (form == null || form.IsDisposed) return;
            Label bar = null;
            foreach (Control c in form.Controls)
                if (c.Name == "uiNotifyBar") bar = (Label)c;
            if (bar == null)
            {
                bar = new Label
                {
                    Name = "uiNotifyBar",
                    Dock = DockStyle.Bottom,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.FromArgb(223, 240, 216),
                    ForeColor = Color.FromArgb(40, 90, 40),
                    Padding = new Padding(0, 0, 12, 0),
                    RightToLeft = RightToLeft.Yes
                };
                var timer = new Timer { Interval = 4000 };
                timer.Tick += (s, e) => { timer.Stop(); bar.Visible = false; };
                bar.Tag = timer;
                form.Controls.Add(bar);
                form.FormClosed += (s, e) => timer.Dispose();
            }
            bar.Text = "✓  " + message;
            bar.Visible = true;
            bar.BringToFront();
            var t = (Timer)bar.Tag;
            t.Stop();
            t.Start();
        }

        /// <summary>
        /// Small dialog that asks for a date (and optionally a note). Returns false when cancelled.
        /// <paramref name="max"/> limits the date (e.g. today for "done on").
        /// </summary>
        public static bool AskDate(IWin32Window owner, string title, string prompt, DateTime initial, DateTime? min, DateTime? max,
            string noteLabel, string initialNote, out DateTime date, out string note)
        {
            date = initial;
            note = initialNote;
            bool withNote = noteLabel != null;
            using (var f = new Form())
            {
                f.Text = title;
                f.RightToLeft = RightToLeft.Yes;
                f.RightToLeftLayout = true;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MinimizeBox = f.MaximizeBox = false;
                f.ShowInTaskbar = false;
                f.Font = new Font("Calibri", 11F);
                f.ClientSize = new Size(440, withNote ? 230 : 150);

                var lbl = new Label { Text = prompt, Location = new Point(15, 12), Size = new Size(410, 44) };
                var picker = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy",
                    Location = new Point(15, 60),
                    Width = 160,
                    RightToLeftLayout = true
                };
                if (min.HasValue) picker.MinDate = min.Value.Date;
                if (max.HasValue) picker.MaxDate = max.Value.Date;
                DateTime start = initial.Date;
                if (min.HasValue && start < min.Value.Date) start = min.Value.Date;
                if (max.HasValue && start > max.Value.Date) start = max.Value.Date;
                picker.Value = start;
                var day = new Label { Location = new Point(185, 63), AutoSize = true, ForeColor = SystemColors.GrayText };
                EventHandler showDay = (s, e) => day.Text = DayName(picker.Value);
                picker.ValueChanged += showDay;
                showDay(null, EventArgs.Empty);
                f.Controls.AddRange(new Control[] { lbl, picker, day });

                TextBox noteBox = null;
                int y = 100;
                if (withNote)
                {
                    f.Controls.Add(new Label { Text = noteLabel, Location = new Point(15, 100), AutoSize = true });
                    noteBox = new TextBox { Location = new Point(15, 125), Width = 410, MaxLength = 200, Text = initialNote ?? "" };
                    f.Controls.Add(noteBox);
                    y = 175;
                }
                var ok = new Button { Text = "אישור", DialogResult = DialogResult.OK, Location = new Point(15, y), Size = new Size(110, 36) };
                var cancel = new Button { Text = "ביטול", DialogResult = DialogResult.Cancel, Location = new Point(135, y), Size = new Size(110, 36) };
                f.Controls.Add(ok);
                f.Controls.Add(cancel);
                f.AcceptButton = ok;
                f.CancelButton = cancel;

                if (f.ShowDialog(owner) != DialogResult.OK) return false;
                date = picker.Value.Date;
                note = noteBox != null ? noteBox.Text.Trim() : null;
                return true;
            }
        }

        /// <summary>Asks for one line of text. Returns null when cancelled.</summary>
        public static string AskText(IWin32Window owner, string title, string prompt, string initial, int maxLength)
        {
            using (var f = new Form())
            {
                f.Text = title;
                f.RightToLeft = RightToLeft.Yes;
                f.RightToLeftLayout = true;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MinimizeBox = f.MaximizeBox = false;
                f.ShowInTaskbar = false;
                f.Font = new Font("Calibri", 11F);
                f.ClientSize = new Size(440, 150);
                var lbl = new Label { Text = prompt, Location = new Point(15, 12), Size = new Size(410, 40) };
                var box = new TextBox { Location = new Point(15, 55), Width = 410, MaxLength = maxLength, Text = initial ?? "" };
                var ok = new Button { Text = "אישור", DialogResult = DialogResult.OK, Location = new Point(15, 100), Size = new Size(110, 36) };
                var cancel = new Button { Text = "ביטול", DialogResult = DialogResult.Cancel, Location = new Point(135, 100), Size = new Size(110, 36) };
                f.Controls.AddRange(new Control[] { lbl, box, ok, cancel });
                f.AcceptButton = ok;
                f.CancelButton = cancel;
                return f.ShowDialog(owner) == DialogResult.OK ? box.Text.Trim() : null;
            }
        }

        public static string DayName(DateTime date)
        {
            return "יום " + new[] { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת" }[(int)date.DayOfWeek];
        }

        public static bool Confirm(string message)
        {
            return MessageBox.Show(message, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, Rtl) == DialogResult.Yes;
        }

        /// <summary>Applies Hebrew headers and hides the columns that are not listed.</summary>
        public static void SetHeaders(DataGridView grid, IDictionary<string, string> headers)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                string header;
                if (headers.TryGetValue(column.Name, out header))
                    column.HeaderText = header;
                else
                    column.Visible = false;
            }
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
        }

        /// <summary>The value of a column in the selected row, or null when nothing is selected.</summary>
        public static object SelectedValue(DataGridView grid, string column)
        {
            DataGridViewRow row = grid.CurrentRow;
            if (row == null && grid.SelectedRows.Count > 0) row = grid.SelectedRows[0];
            if (row == null) return null;
            return row.Cells[column].Value;
        }

        public static int? SelectedInt(DataGridView grid, string column)
        {
            object value = SelectedValue(grid, column);
            return value == null ? (int?)null : Convert.ToInt32(value);
        }

        public static string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : string.Empty;
        }

        public static string FormatMoney(decimal amount)
        {
            return amount.ToString("#,##0.00") + " ₪";
        }
    }
}
