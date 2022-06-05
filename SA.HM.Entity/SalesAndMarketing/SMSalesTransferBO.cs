using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesTransferBO
    {
        public long SalesTransferId { get; set; }
        public Nullable<long> DealId { get; set; }
        public string DealName { get; set; }
        public long QuotationId { get; set; }
        public string QuotationNo { get; set; }
        public int CompanyId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationID { get; set; }
        public string CompanyName { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
        public string TransferNumber { get; set; }

    }
}
