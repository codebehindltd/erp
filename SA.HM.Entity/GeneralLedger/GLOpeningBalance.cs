using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLOpeningBalance
    {
        public long Id { get; set; }
        public string TransactionType { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int FiscalYearId { get; set; }
        public Nullable<System.DateTime> OpeningBalanceDate { get; set; }
        public Nullable<System.Decimal> OpeningBalanceEquity { get; set; }
        public bool IsApproved { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
    }
}
