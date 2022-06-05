using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ChequeListStatReportViewBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string VoucherDate { get; set; }
        public string NodeHead { get; set; }
        public string Narration { get; set; }
        public string ChequeNumber { get; set; }
        public string VoucherNo { get; set; }
        public decimal LedgerAmount { get; set; }
    }
}
