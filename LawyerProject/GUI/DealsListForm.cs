using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Property transactions: cases that are not managed in court.</summary>
    public partial class DealsListForm : Form
    {
        private class DealRow
        {
            public int CaseId { get; set; }
            public string Client { get; set; }
            public string PropertyAddress { get; set; }
            public string Block { get; set; }
            public string Parcel { get; set; }
            public string SubParcel { get; set; }
            public decimal? PropertyValue { get; set; }
            public decimal Balance { get; set; }
        }

        private List<DealRow> all = new List<DealRow>();

        public DealsListForm()
        {
            InitializeComponent();
            LoadDeals();
        }

        private void LoadDeals()
        {
            using (var db = new QuedinDbContext())
            {
                all = db.Cases
                    .Include(t => t.Client)
                    .Include(t => t.CaseType)
                    .Include(t => t.Payments)
                    .Where(t => t.IsInCourt == false)
                    .ToList()
                    .Select(t => new DealRow
                    {
                        CaseId = t.CaseId,
                        Client = t.Client != null ? t.Client.ToString() : "",
                        PropertyAddress = t.PropertyAddress,
                        Block = t.Block,
                        Parcel = t.Parcel,
                        SubParcel = t.SubParcel,
                        PropertyValue = t.ClaimAmount,
                        Balance = CaseService.OpenBalance(t)
                    })
                    .OrderByDescending(r => r.CaseId)
                    .ToList();
            }
            ApplyFilter();
        }

        private static bool Matches(string value, TextBox box)
        {
            string text = box.Text.Trim();
            return text.Length == 0 || (value ?? "").StartsWith(text);
        }

        private void ApplyFilter()
        {
            dataGridView1.DataSource = all
                .Where(r => (txtPropertyAddress.Text.Trim().Length == 0 || (r.PropertyAddress ?? "").Contains(txtPropertyAddress.Text.Trim()))
                         && Matches(r.Block, txtBlock)
                         && Matches(r.Parcel, txtParcel)
                         && Matches(r.SubParcel, txtSubParcel))
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "CaseId", "מספר תיק" },
                { "Client", "לקוח" },
                { "PropertyAddress", "כתובת נכס" },
                { "Block", "גוש" },
                { "Parcel", "חלקה" },
                { "SubParcel", "תת חלקה" },
                { "PropertyValue", "שווי נכס" },
                { "Balance", "יתרה לתשלום" }
            });
        }

        private void txtPropertyAddress_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtBlock_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtParcel_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtSubParcel_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            CaseWindows.NewCase(this, true, () => { if (!IsDisposed) LoadDeals(); });
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "CaseId");
            if (id == null)
            {
                Ui.Error("יש לבחור עסקה מהרשימה.");
                return;
            }
            CaseWindows.OpenCase(this, id.Value, () => { if (!IsDisposed) LoadDeals(); });
        }
    }
}
