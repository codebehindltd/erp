using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.RetailPOS
{
    public class RetailPosSalesReturnItemBO
    {
        public Nullable<int> BillId { get; set; }
        public Nullable<int> ReturnBillId { get; set; }
        public string ReturnBillNumber { get; set; }
        public Nullable<System.DateTime> ReturnBillDate { get; set; }
        public Nullable<decimal> ExchangeItemVatAmount { get; set; }
        public Nullable<decimal> ExchangeItemTotal { get; set; }
        public int KotId { get; set; }
        public Nullable<int> BearerId { get; set; }
        public Nullable<int> ReferenceKotId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> ItemUnit { get; set; }
        public Nullable<decimal> UnitRate { get; set; }    
        public Nullable<decimal> ItemTotalAmount { get; set; }
        public Nullable<decimal> PointsRedeemed { get; set; }
        public Nullable<decimal> PreviousDIscount { get; set; }
        
    }
}
