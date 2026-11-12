using System;

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
