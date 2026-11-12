using System;
using LawyerProject.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.Tests
{
    [TestClass]
    public class FeeCalculatorTests
    {
        [TestMethod]
        public void CaseTotal_IsPercentOfClaimPlusCourtFee()
        {
            Assert.AreEqual(18200m, FeeCalculator.CaseTotal(85000m, 20, 1200m));
        }

        [TestMethod]
        public void CaseTotal_RoundsToAgorot()
        {
            Assert.AreEqual(333.33m, FeeCalculator.CaseTotal(1000m, 33.333, 0m));
        }

        [TestMethod]
        public void CaseTotal_RejectsInvalidInput()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => FeeCalculator.CaseTotal(-1m, 10, 0m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => FeeCalculator.CaseTotal(100m, 101, 0m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => FeeCalculator.CaseTotal(100m, 10, -5m));
        }

        [TestMethod]
        public void BalanceAfterPayment_Subtracts()
        {
            Assert.AreEqual(1500.50m, FeeCalculator.BalanceAfterPayment(2000m, 499.50m));
            Assert.AreEqual(0m, FeeCalculator.BalanceAfterPayment(2000m, 2000m));
        }

        [TestMethod]
        public void BalanceAfterPayment_RejectsOverpaymentAndZero()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => FeeCalculator.BalanceAfterPayment(100m, 100.01m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => FeeCalculator.BalanceAfterPayment(100m, 0m));
        }

        [TestMethod]
        public void SplitVat_At18Percent()
        {
            VatBreakdown b = FeeCalculator.SplitVat(1180m, 0.18m);

            Assert.AreEqual(1000m, b.Net);
            Assert.AreEqual(180m, b.Vat);
            Assert.AreEqual(1180m, b.Total);
        }

        [TestMethod]
        public void SplitVat_PartsAlwaysAddUpToTotal()
        {
            foreach (decimal total in new[] { 0.01m, 99.99m, 1234.56m, 18200m, 7777.77m })
            {
                VatBreakdown b = FeeCalculator.SplitVat(total, 0.18m);
                Assert.AreEqual(b.Total, b.Net + b.Vat, "total " + total);
            }
        }

        [TestMethod]
        public void Balance_IsTotalMinusAllPayments_IncludingCancellations()
        {
            Assert.AreEqual(18200m, FeeCalculator.Balance(18200m, new decimal[0]));
            Assert.AreEqual(10200m, FeeCalculator.Balance(18200m, new[] { 5000m, 3000m }));
            Assert.AreEqual(13200m, FeeCalculator.Balance(18200m, new[] { 5000m, 3000m, -3000m }), "a cancellation returns the amount");
            Assert.AreEqual(0m, FeeCalculator.Balance(100.10m, new[] { 50.05m, 50.05m }));
        }

        [TestMethod]
        public void Balance_FollowsTheCurrentTotal_WhenTheClaimChanges()
        {
            // The old version kept the balance from the last payment, so raising the claim was ignored.
            decimal before = FeeCalculator.Balance(FeeCalculator.CaseTotal(85000m, 20, 1200m), new[] { 5000m });
            decimal after = FeeCalculator.Balance(FeeCalculator.CaseTotal(100000m, 20, 1200m), new[] { 5000m });
            Assert.AreEqual(13200m, before);
            Assert.AreEqual(16200m, after);
        }

        [TestMethod]
        public void ReceiptVat_FeeIncludesVat_ExpensesDoNot()
        {
            VatBreakdown fee = FeeCalculator.ReceiptVat(1180m, PaymentKinds.Fee, 0.18m);
            VatBreakdown expenses = FeeCalculator.ReceiptVat(1200m, PaymentKinds.Expenses, 0.18m);

            Assert.AreEqual(180m, fee.Vat);
            Assert.AreEqual(0m, expenses.Vat);
            Assert.AreEqual(1200m, expenses.Net);
        }

        [TestMethod]
        public void ReceiptVat_UsesTheRateStoredWithThePayment()
        {
            Assert.AreEqual(170m, FeeCalculator.ReceiptVat(1170m, PaymentKinds.Fee, 0.17m).Vat);
        }

        [TestMethod]
        public void ReceiptVat_CreditNoteIsNegativeAndAddsUp()
        {
            VatBreakdown b = FeeCalculator.ReceiptVat(-1180m, PaymentKinds.Fee, 0.18m);
            Assert.AreEqual(-180m, b.Vat);
            Assert.AreEqual(b.Total, b.Net + b.Vat);
        }
    }
}
