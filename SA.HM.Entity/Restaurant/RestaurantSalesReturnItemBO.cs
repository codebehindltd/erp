using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantSalesReturnItemBO
    {
        public long ReturnId { get; set; }
        public int BillId { get; set; }
        public int KotId { get; set; }
        public int KotDetailId { get; set; }
        public int CostCenterId { get; set; }
        public string BillNumber { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Amount { get; set; }
        public decimal AverageCost { get; set; }
        public int CreatedBy { get; set; }
        public decimal ReturnedUnit { get; set; }
        public decimal InvoiceDiscount { get; set; }

    }
}
