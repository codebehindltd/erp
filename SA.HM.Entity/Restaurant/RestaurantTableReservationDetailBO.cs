using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantTableReservationDetailBO
    {
        public int ReservationDetailId { get; set; }
        public int ReservationId { get; set; }
        public int CostCenterId { get; set; }
        public int TableId { get; set; }
        public string DiscountType { get; set; }
        public decimal Amount { get; set; }
        public bool IsRegistered { get; set; }

        //Extra field for session save
        public string CostCentre { get; set; }
        public string TableNumber { get; set; }
        public int NumberOfTable { get; set; }
        public int TableQuantity { get; set; }
        public string TableNumberIdList { get; set; }
        public string TableNumberList { get; set; }
        public string TableNumberListInfoWithCount { get; set; }
    }
}
