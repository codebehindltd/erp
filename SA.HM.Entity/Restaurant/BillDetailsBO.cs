using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class BillDetailsBO
    {
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string BillTime { get; set; }
        public Nullable<decimal> RoundedGrandTotal { get; set; }
        public int KotDetailId { get; set; }
        public int KotId { get; set; }
        public string KotTime { get; set; }
        public string Cashier { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UserInfoId { get; set; }
        public Nullable<int> CostCenterId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string CategoryName { get; set; }
        public string SettlementDetails { get; set; }
        public string PaymentType { get; set; }
        public string CostCenter { get; set; }
        public string BillingSession { get; set; }

    }
}
