using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Records a payment for a case and updates its open balance.
    /// Card payments keep only the last 4 digits (no full number, expiry or CVV).
    /// </summary>
    public partial class frmAddTashlumimLetik : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly Tikim_Letipul_Orech_Din tik;
        private readonly decimal openBalance;

        public frmAddTashlumimLetik(int tikId)
        {
            InitializeComponent();
            tik = db.Tikim_Letipul_Orech_Din.FirstOrDefault(t => t.IDTik == tikId);
            if (tik == null) throw new ArgumentException("Case not found: " + tikId);

            openBalance = CaseService.OpenBalance(tik);
            lblTik.Text = tik.IDTik + " - " + (tik.Customers != null ? tik.Customers.ToString() : "");
            lblBalance.Text = Ui.FormatMoney(openBalance);

            cmbEmzhaeyTashlum.Items.AddRange(new object[]
            {
                PaymentMethods.Cash, PaymentMethods.CreditCard, PaymentMethods.BankTransfer, PaymentMethods.Check
            });
            cmbEmzhaeyTashlum.SelectedItem = PaymentMethods.Cash;
            dateTimePickerTashlum.Value = DateTime.Today;
            dateTimePickerTashlum.MaxDate = DateTime.Today;
            dateTimePickerDateChek.Value = DateTime.Today;

            if (openBalance <= 0)
            {
                Ui.Info("התיק שולם במלואו. אין יתרה לתשלום.");
                btnOK.Enabled = false;
            }
        }

        private string Method
        {
            get { return cmbEmzhaeyTashlum.SelectedItem as string; }
        }

        private void cmbEmzhaeyTashlum_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxCartisAshray.Visible = Method == PaymentMethods.CreditCard;
            groupBoxAavaraBankait.Visible = Method == PaymentMethods.BankTransfer;
            groupBoxChek.Visible = Method == PaymentMethods.Check;
        }

        private void txtSchum_TextChanged(object sender, EventArgs e)
        {
            decimal amount;
            lblItraAfter.Text = Validation.TryParseMoney(txtSchum.Text, out amount) && amount > 0 && amount <= openBalance
                ? Ui.FormatMoney(openBalance - amount)
                : "-";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            decimal amount;
            if (!Validation.TryParseMoney(txtSchum.Text, out amount) || amount <= 0) fail(txtSchum, "יש להזין סכום");
            else if (amount > openBalance) fail(txtSchum, "הסכום גבוה מהיתרה (" + Ui.FormatMoney(openBalance) + ")");

            if (Method == null) fail(cmbEmzhaeyTashlum, "יש לבחור אמצעי תשלום");
            else if (Method == PaymentMethods.CreditCard)
            {
                if (txtCardLast4.Text.Trim().Length != 4) fail(txtCardLast4, "יש להזין 4 ספרות");
            }
            else if (Method == PaymentMethods.BankTransfer)
            {
                if (txtNumAsmachta.Text.Trim().Length == 0) fail(txtNumAsmachta, "יש להזין מספר אסמכתא");
            }
            else if (Method == PaymentMethods.Check)
            {
                if (txtnumchek.Text.Trim().Length == 0) fail(txtnumchek, "יש להזין מספר צ'ק");
                if (txtNumBank.Text.Trim().Length == 0) fail(txtNumBank, "יש להזין מספר בנק");
                if (txtNumSnif.Text.Trim().Length == 0) fail(txtNumSnif, "יש להזין מספר סניף");
                if (txtnumaccount.Text.Trim().Length == 0) fail(txtnumaccount, "יש להזין מספר חשבון");
            }
            if (!ok) return;

            var payment = new Tashlumim_Letik
            {
                IDTik = tik.IDTik,
                Schum = FeeCalculator.Round(amount),
                Itra = FeeCalculator.BalanceAfterPayment(openBalance, amount),
                TaarichTashlum = dateTimePickerTashlum.Value.Date,
                EmzhaeyTashlum = Method,
                Teur = txtTeur.Text.Trim().Length == 0 ? null : txtTeur.Text.Trim()
            };

            if (Method == PaymentMethods.CreditCard)
                payment.NumCartis = "****" + txtCardLast4.Text.Trim();
            else if (Method == PaymentMethods.BankTransfer)
                payment.NumAsmachta = txtNumAsmachta.Text.Trim();
            else if (Method == PaymentMethods.Check)
            {
                payment.NumChek = txtnumchek.Text.Trim();
                payment.NumBank = txtNumBank.Text.Trim();
                payment.NumSnif = txtNumSnif.Text.Trim();
                payment.NumCheshbon = txtnumaccount.Text.Trim();
                payment.TaarichLachek = dateTimePickerDateChek.Value.Date;
            }

            try
            {
                db.Tashlumim_Letik.Add(payment);
                if (payment.Itra == 0) tik.Shulam = true;
                db.SaveChanges();

                if (Ui.Confirm("התשלום נשמר. להציג קבלה?"))
                    using (var receipt = new frmKabala(payment.IDTashlum))
                        receipt.ShowDialog();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! התשלום לא נשמר.", ex);
            }
        }

        private void Money_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsMoneyKey(e.KeyChar);
        }

        private void Digits_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsDigitKey(e.KeyChar);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
