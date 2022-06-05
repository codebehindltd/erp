using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelGuestHouseServiceBillBO
    {
        public string PrintDate { get; set; }
        public int ServiceBillId { get; set; }
        public string ServiceDate { get; set; }
        public string BillNumber { get; set; }
        public string GuestName { get; set; }
        public Nullable<decimal> SalesAmount { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> ServiceQuantity { get; set; }
        public Nullable<decimal> ServiceRate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public int IsInclusiveBill { get; set; }
        public int IsVatServiceChargeEnable { get; set; }
    }
}
