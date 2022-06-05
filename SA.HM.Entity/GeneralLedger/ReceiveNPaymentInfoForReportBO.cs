using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ReceiveNPaymentInfoForReportBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public long CBCode { get; set; }
        public Nullable<byte> PartNo { get; set; }
        public string VoucherMode { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<int> NodeId { get; set; }
        public string NodeHead { get; set; }
        public string HierarchyIndex { get; set; }
        public Nullable<decimal> PriorBalance { get; set; }
        public Nullable<decimal> ReceivedAmount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string VoucherDate { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
