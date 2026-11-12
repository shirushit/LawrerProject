using System;
using System.Windows.Forms;
using LawyerProject.BLL;

namespace LawyerProject.GUI
{
    /// <summary>Home screen. Management screens are shown to managers only.</summary>
    public partial class Nivot : Form
    {
        public Nivot()
        {
            InitializeComponent();

            bool manager = Session.IsManager;
            btnCourts.Visible = manager;
            btnCaseTypes.Visible = manager;
            btnProcesses.Visible = manager;
            btnUsers.Visible = manager;

            if (Session.CurrentUser != null)
                lblWelcome.Text = "שלום " + Session.CurrentUser.UserName + " (" + Session.CurrentUser.KindWorker + ")";
        }

        private static void Open(Form form)
        {
            using (form)
                form.ShowDialog();
        }

        private void btnAbout_Click(object sender, EventArgs e) { Open(new FrmAbout()); }

        private void btnCustomers_Click(object sender, EventArgs e) { Open(new frmAllCostemers()); }

        private void btnTikim_Click(object sender, EventArgs e) { Open(new frmAllTikimLetipulOrechDin()); }

        private void btnIskaot_Click(object sender, EventArgs e) { Open(new frmAllIskaot()); }

        private void btnKabalot_Click(object sender, EventArgs e) { Open(new FrmAllPayments()); }

        private void btnCourts_Click(object sender, EventArgs e) { Open(new frmAllBetMishpat()); }

        private void btnCaseTypes_Click(object sender, EventArgs e) { Open(new frmAllSubjektTik()); }

        private void btnProcesses_Click(object sender, EventArgs e) { Open(new FrmAllTaalichimLetik()); }

        private void btnUsers_Click(object sender, EventArgs e) { Open(new FrmManageUsers()); }

        private void btnLogout_Click(object sender, EventArgs e) { Close(); }
    }
}
