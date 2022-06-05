using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class SupplierPaymentLedgerVwBO
    {
        public int? SupplierId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ShowPaymentDate { get; set; }
        public string Narration { get; set; }
        public decimal? DRAmount { get; set; }
        public decimal? CRAmount { get; set; }
        public decimal? ClosingBalance { get; set; }

        public decimal? BalanceCommulative { get; set; }

        public string SupplierName { get; set; }
        public string CurrencyName { get; set; }
    }
}
