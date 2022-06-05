using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantReservationItemDetailBO
    {
        public long ItemDetailId { get; set; }
        public long ReservationId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsComplementary { get; set; }
    }
}
