using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLLedgerMasterVwBO : GLLedgerMasterBO
    {
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string VoucherTypeName { get; set; }
        public string DonorName { get; set; }

        public string CurrencyName { get; set; }
        public int CanEditDeleteAfterApproved { get; set; }
        public string VoucherDateString { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanCheck { get; set; }
        public bool IsCanApprove { get; set; }
        public decimal VoucherTotalAmount { get; set; }
        public bool IsModulesTransaction { get; set; }
    }
}
