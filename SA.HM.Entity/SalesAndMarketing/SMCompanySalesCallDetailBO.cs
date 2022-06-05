using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCompanySalesCallDetailBO
    {
        public int SalesCallDetailId { get; set; }
        public int SalesCallId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
