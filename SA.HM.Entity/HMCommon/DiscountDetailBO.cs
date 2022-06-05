using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DiscountDetailBO
    {
        public long Id { get; set; }
        public long DiscountMasterId { get; set; }
        public long DiscountForId { get; set; }
        public string DiscountType { get; set; }
        public decimal Discount { get; set; }

        public string DiscountForName { get; set; }
    }
}
