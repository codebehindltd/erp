using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyWiseDiscountPolicyBO
    {
        public long CompanyWiseDiscountId { get; set; }
        public int CompanyId { get; set; }
        public int RoomTypeId { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool ActiveStat { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string ActiveStatus { get; set; }
        public string RoomType { get; set; }
    }
}
