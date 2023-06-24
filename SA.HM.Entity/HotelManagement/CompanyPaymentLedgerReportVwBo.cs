using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentLedgerReportVwBo : HotelCompanyPaymentLedgerBO
    {
        public DateTime? PaymentDate { get; set; }
        public string Narration { get; set; }
        public decimal? ClosingBalance { get; set; }
        public decimal? BalanceCommulative { get; set; }
        public string ContactName { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> PreviousYearClosingBalance { get; set; }
        public Nullable<decimal> CurrentYearClosingBalance { get; set; }
    }
}
