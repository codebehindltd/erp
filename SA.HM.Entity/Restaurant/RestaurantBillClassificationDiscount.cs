using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillClassificationDiscount
    {
        [Key]
        public int DiscountId { get; set; }

        public int? BillId { get; set; }

        public int? ClassificationId { get; set; }
        
        public decimal? DiscountAmount { get; set; }
    }
}
