using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
   public  class TimeSlabHeadBO
    {
        public int TimeSlabId { get; set; }
        public string TimeSlabHead { get; set; }
        public int SecondTimeSlabId { get; set; }
        public string SecondTimeSlabHead { get; set; }
        public DateTime SlabStartTime { get; set; }
        public string SlabStartTimeDisplay { get; set; }
        public DateTime SlabEndTime { get; set; }
        public string SlabEndTimeDisplay { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string StartHour { get; set; }
        public string StartMin { get; set; }
        public string StartAMPM { get; set; }
        public string EndHour { get; set; }
        public string EndMin { get; set; }
        public string EndAMPM { get; set; }
    }
}
