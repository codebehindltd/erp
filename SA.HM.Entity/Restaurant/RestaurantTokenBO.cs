using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantTokenBO
    {
        public long TokenId { get; set; }
        public int CostCenterId { get; set; }
        public int BearerId { get; set; }
        public DateTime TokenDate { get; set; }
        public string TokenNumber { get; set; }
        public int? KotId { get; set; }
        public int? BillId { get; set; }
        public bool IsBillHoldup { get; set; }
        public string TokenStatus { get; set; }
        public int SourceId { get; set; }
    }
}
