using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantKotSpecialRemarksDetailBO
    {
        public int RemarksDetailId { get; set; }
        public int KotId { get; set; }
        public int ItemId { get; set; }
        public int SpecialRemarksId { get; set; }

        public string SpecialRemarks { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
