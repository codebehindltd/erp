using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class OnlineMemberEducationBO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int? PassingYear { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
