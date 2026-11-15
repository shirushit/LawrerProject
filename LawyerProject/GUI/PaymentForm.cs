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
    public partial class PaymentForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private readonly LegalCase legalCase;
        private readonly decimal openBalance;

        public PaymentForm(int caseId)
        {
            InitializeComponent();
            legalCase = db.Cases.FirstOrDefault(t => t.CaseId == caseId);
            if (legalCase == null) throw new ArgumentException("Case not found: " + caseId);

            openBalance = CaseService.OpenBalance(legalCase);
            lblCase.Text = legalCase.CaseId + " - " + (legalCase.Client != null ? legalCase.Client.ToString() : "");
            lblBalance.Text = Ui.FormatMoney(openBalance);

            cmbMethod.Items.AddRange(new object[]
            {
                PaymentMethods.Cash, PaymentMethods.CreditCard, PaymentMethods.BankTransfer, PaymentMethods.Check
            });
            cmbMethod.SelectedItem = PaymentMethods.Cash;
            cmbKind.Items.AddRange(PaymentKinds.All);
            cmbKind.SelectedItem = PaymentKinds.Fee;
            dtpPaidOn.Value = Clock.Today;
            dtpPaidOn.MaxDate = Clock.Today;
            dtpChequeDate.Value = Clock.Today;

            if (openBalance <= 0)
            {
                lblBalanceAfter.Text = "התיק שולם במלואו - אין יתרה לתשלום.";
                btnOK.Enabled = false;
            }
            FitToMethod();
        }

        /// <summary>The id of the saved payment (receipt number), after DialogResult.OK.</summary>
        public int SavedPaymentId { get; private set; }

        private void cmbKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblKindNote.Text = cmbKind.SelectedItem as string == PaymentKinds.Expenses ? "ללא מע\"מ" : "כולל מע\"מ";
        }

        /// <summary>Shrinks or grows the window to the details group of the chosen method, so no empty area is left.</summary>
        private void FitToMethod()
        {
            GroupBox shown = new[] { groupBoxCard, groupBoxBankTransfer, groupBoxCheque }.FirstOrDefault(g => g.Visible);
            int bottom = shown != null ? shown.Bottom : groupBoxCard.Top - 10;
            btnOK.Top = bottom + 15;
            ClientSize = new System.Drawing.Size(ClientSize.Width, btnOK.Bottom + 20);
        }

        private string Method
        {
            get { return cmbMethod.SelectedItem as string; }
        }

        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxCard.Visible = Method == PaymentMethods.CreditCard;
            groupBoxBankTransfer.Visible = Method == PaymentMethods.BankTransfer;
            groupBoxCheque.Visible = Method == PaymentMethods.Check;
            FitToMethod();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            decimal amount;
            lblBalanceAfter.Text = Validation.TryParsePayment(txtAmount.Text, out amount) && amount <= openBalance
                ? Ui.FormatMoney(openBalance - amount)
                : "-";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            decimal amount;
            if (!Validation.TryParseMoney(txtAmount.Text, out amount)) fail(txtAmount, "סכום לא תקין (לדוגמה 1,250.50)");
            else if (amount < 0.01m) fail(txtAmount, "הסכום חייב להיות לפחות 0.01 ₪");
            else if (amount > openBalance) fail(txtAmount, "הסכום גבוה מהיתרה (" + Ui.FormatMoney(openBalance) + ")");

            if (cmbKind.SelectedItem == null) fail(cmbKind, "יש לבחור סוג תשלום");
            if (Method == null) fail(cmbMethod, "יש לבחור אמצעי תשלום");
            else if (Method == PaymentMethods.CreditCard)
            {
                if (txtCardLast4.Text.Trim().Length != 4) fail(txtCardLast4, "יש להזין 4 ספרות");
            }
            else if (Method == PaymentMethods.BankTransfer)
            {
                if (txtReference.Text.Trim().Length == 0) fail(txtReference, "יש להזין מספר אסמכתא");
            }
            else if (Method == PaymentMethods.Check)
            {
                if (txtChequeNumber.Text.Trim().Length == 0) fail(txtChequeNumber, "יש להזין מספר צ'ק");
                if (txtBankNumber.Text.Trim().Length == 0) fail(txtBankNumber, "יש להזין מספר בנק");
                if (txtBranchNumber.Text.Trim().Length == 0) fail(txtBranchNumber, "יש להזין מספר סניף");
                if (txtAccountNumber.Text.Trim().Length == 0) fail(txtAccountNumber, "יש להזין מספר חשבון");
            }
            if (!ok) return;

            var payment = new Payment
            {
                Amount = amount,
                Kind = (string)cmbKind.SelectedItem,
                PaidOn = dtpPaidOn.Value.Date,
                Method = Method,
                Description = txtDescription.Text.Trim().Length == 0 ? null : txtDescription.Text.Trim()
            };

            if (Method == PaymentMethods.CreditCard)
                payment.CardLast4 = "****" + txtCardLast4.Text.Trim();
            else if (Method == PaymentMethods.BankTransfer)
                payment.Reference = txtReference.Text.Trim();
            else if (Method == PaymentMethods.Check)
            {
                payment.ChequeNumber = txtChequeNumber.Text.Trim();
                payment.BankNumber = txtBankNumber.Text.Trim();
                payment.BranchNumber = txtBranchNumber.Text.Trim();
                payment.AccountNumber = txtAccountNumber.Text.Trim();
                payment.ChequeDate = dtpChequeDate.Value.Date;
            }

            try
            {
                // Balance, VAT rate, "paid" flags and the audit entry are handled by the service.
                string error = new CaseService(db).AddPayment(legalCase, payment);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                SavedPaymentId = payment.PaymentId;

                if (Ui.Confirm("התשלום נשמר (קבלה מספר " + payment.PaymentId + "). להציג את הקבלה להדפסה?"))
                    using (var receipt = new ReceiptForm(payment.PaymentId))
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
