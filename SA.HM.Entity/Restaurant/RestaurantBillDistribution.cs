using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillDistribution
    {
        public Nullable<long> TransactionId { get; set; }
        public Nullable<decimal> RackRate { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> SDCityCharge { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> AdditionalCharge { get; set; }
        public Nullable<decimal> CalculatedAmount { get; set; }
    }
}
