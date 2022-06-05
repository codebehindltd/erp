using HotelManagement.Entity.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class RestaurantBillSyncBO: RestaurantBillBO
    {
        public long Id { get; set; }
        public long BillId { get; set; }
        public long SyncId { get; set; }
        public string Status { get; set; }
    }
}
