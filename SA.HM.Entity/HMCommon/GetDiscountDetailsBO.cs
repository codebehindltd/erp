using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class GetDiscountDetailsBO
    {
        public long Id { get; set; }
        public long DiscountMasterId { get; set; }
        public long DiscountForId { get; set; }
        public string DiscountType { get; set; }
        public decimal Discount { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountName { get; set; }
    }
}
