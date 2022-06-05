using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetInformationBO
    {
        public int CostCenterId { get; set; }
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public Int64 CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public Int64 LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }

        public int IsVatSChargeInclusive { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal VatAmount { get; set; }
        public long AccountsPostingHeadId { get; set; }
        public long ExpenseAccountsPostingHeadId { get; set; }
    }
}
