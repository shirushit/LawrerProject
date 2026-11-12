using System;
using System.Collections.Generic;
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

        public static void Error(string message, Exception ex)
        {
            Error(message + Environment.NewLine + ex.GetBaseException().Message);
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
