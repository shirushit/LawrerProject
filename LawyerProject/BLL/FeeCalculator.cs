using System;
using System.Collections.Generic;
using System.Linq;

namespace LawyerProject.BLL
{
    /// <summary>Breakdown of a VAT-inclusive amount, as printed on a receipt.</summary>
    public class VatBreakdown
    {
        public decimal Net { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>Money rules for cases and receipts. Pure logic, unit-tested.</summary>
    public static class FeeCalculator
    {
        /// <summary>
        /// Total the office charges for a case: the agreed percentage of the
        /// claim (or property value) plus the court fee for the case type.
        /// </summary>
        public static decimal CaseTotal(decimal claimAmount, double profitPercent, decimal courtFee)
        {
            if (claimAmount < 0) throw new ArgumentOutOfRangeException(nameof(claimAmount));
            if (profitPercent < 0 || profitPercent > 100) throw new ArgumentOutOfRangeException(nameof(profitPercent));
            if (courtFee < 0) throw new ArgumentOutOfRangeException(nameof(courtFee));

            decimal share = claimAmount * (decimal)profitPercent / 100m;
            return Round(share + courtFee);
        }

        /// <summary>
        /// The open balance of a case, always computed: case total minus the sum of its payments.
        /// Cancellations are stored as negative payments, so they add back automatically.
        /// </summary>
        public static decimal Balance(decimal caseTotal, IEnumerable<decimal> payments)
        {
            decimal paid = payments == null ? 0m : payments.Sum();
            return Round(caseTotal - paid);
        }

        /// <summary>
        /// VAT shown on a receipt. Only fee payments carry VAT; court fees and expenses passed on to
        /// the client do not. <paramref name="vatRate"/> is the rate stored with the payment.
        /// </summary>
        public static VatBreakdown ReceiptVat(decimal amount, string paymentKind, decimal vatRate)
        {
            if (paymentKind == PaymentKinds.Expenses)
                return new VatBreakdown { Net = Round(amount), Vat = 0m, Total = Round(amount) };
            return SplitVat(amount, vatRate);
        }

        /// <summary>Balance left after a payment. A payment may not exceed the open balance.</summary>
        public static decimal BalanceAfterPayment(decimal openBalance, decimal payment)
        {
            if (payment <= 0) throw new ArgumentOutOfRangeException(nameof(payment), "סכום התשלום חייב להיות גדול מאפס.");
            if (payment > openBalance) throw new ArgumentOutOfRangeException(nameof(payment), "הסכום גבוה מהיתרה לתשלום.");
            return Round(openBalance - payment);
        }

        /// <summary>Splits a VAT-inclusive total into net amount and VAT.</summary>
        public static VatBreakdown SplitVat(decimal totalIncludingVat, decimal vatRate)
        {
            if (vatRate < 0 || vatRate >= 1) throw new ArgumentOutOfRangeException(nameof(vatRate));
            decimal net = Round(totalIncludingVat / (1 + vatRate));
            return new VatBreakdown
            {
                Net = net,
                Vat = Round(totalIncludingVat) - net,
                Total = Round(totalIncludingVat)
            };
        }

        public static decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
