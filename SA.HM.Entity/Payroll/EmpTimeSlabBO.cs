using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTimeSlabBO
    {
        public int EmpTimeSlabId { get; set; }
        public int EmpId { get; set; }//relative
        public DateTime SlabEffectDate { get; set; }
        public string EffectDate { get; set; }
        public int TimeSlabId { get; set; }//relative
        public string WeekEndMode { get; set; }
        public string WeekEndFirst { get; set; }
        public string WeekEndSecond { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
