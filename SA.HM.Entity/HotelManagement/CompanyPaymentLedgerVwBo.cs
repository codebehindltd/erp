using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentLedgerVwBo : HotelCompanyPaymentLedgerBO
    {
        public string Narration { get; set; }
        public decimal? ClosingBalance { get; set; }
        public decimal? BalanceCommulative { get; set; }
        public string ContactName { get; set; }
        public string CurrencyName { get; set; }
        public string CompanyBillNumber { get; set; }
        public long CompanyBillDetailsId { get; set; }

        public DateTime BillDate { get; set; }
    }
}
