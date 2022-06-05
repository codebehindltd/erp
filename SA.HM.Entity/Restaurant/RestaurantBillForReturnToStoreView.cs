using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillForReturnToStoreView
    {
        public int BillId { get; set; }

        public string BillNumber { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int CostCenterId { get; set; }

        public string CostCenter { get; set; }

        public int LocationId { get; set; }

        public string Location { get; set; }
    }
}
