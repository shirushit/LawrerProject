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
            cmbSugTashlum.Items.AddRange(PaymentKinds.All);
            cmbSugTashlum.SelectedItem = PaymentKinds.Fee;
            dateTimePickerTashlum.Value = DateTime.Today;
            dateTimePickerTashlum.MaxDate = DateTime.Today;
            dateTimePickerDateChek.Value = DateTime.Today;

            if (openBalance <= 0)
            {
                lblItraAfter.Text = "התיק שולם במלואו - אין יתרה לתשלום.";
                btnOK.Enabled = false;
            }
            FitToMethod();
        }

        /// <summary>The id of the saved payment (receipt number), after DialogResult.OK.</summary>
        public int SavedPaymentId { get; private set; }

        private void cmbSugTashlum_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblKindNote.Text = cmbSugTashlum.SelectedItem as string == PaymentKinds.Expenses ? "ללא מע\"מ" : "כולל מע\"מ";
        }

        /// <summary>Shrinks or grows the window to the details group of the chosen method, so no empty area is left.</summary>
        private void FitToMethod()
        {
            GroupBox shown = new[] { groupBoxCartisAshray, groupBoxAavaraBankait, groupBoxChek }.FirstOrDefault(g => g.Visible);
            int bottom = shown != null ? shown.Bottom : groupBoxCartisAshray.Top - 10;
            btnOK.Top = bottom + 15;
            ClientSize = new System.Drawing.Size(ClientSize.Width, btnOK.Bottom + 20);
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
            FitToMethod();
        }

        private void txtSchum_TextChanged(object sender, EventArgs e)
        {
            decimal amount;
            lblItraAfter.Text = Validation.TryParsePayment(txtSchum.Text, out amount) && amount <= openBalance
                ? Ui.FormatMoney(openBalance - amount)
                : "-";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            decimal amount;
            if (!Validation.TryParseMoney(txtSchum.Text, out amount)) fail(txtSchum, "סכום לא תקין (לדוגמה 1,250.50)");
            else if (amount < 0.01m) fail(txtSchum, "הסכום חייב להיות לפחות 0.01 ₪");
            else if (amount > openBalance) fail(txtSchum, "הסכום גבוה מהיתרה (" + Ui.FormatMoney(openBalance) + ")");

            if (cmbSugTashlum.SelectedItem == null) fail(cmbSugTashlum, "יש לבחור סוג תשלום");
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
                Schum = amount,
                SugTashlum = (string)cmbSugTashlum.SelectedItem,
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
                // Balance, VAT rate, "paid" flags and the audit entry are handled by the service.
                string error = new CaseService(db).AddPayment(tik, payment);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                SavedPaymentId = payment.IDTashlum;

                if (Ui.Confirm("התשלום נשמר (קבלה מספר " + payment.IDTashlum + "). להציג את הקבלה להדפסה?"))
                    using (var receipt = new frmKabala(payment.IDTashlum))
                        receipt.ShowDialog(this);

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
