using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpRosterBO
    {
        public int EmpRosterId { get; set; }
        public int RosterId { get; set; }
        public string RosterName { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public DateTime RosterDate { get; set; }
        public int TimeSlabId { get; set; }
        public string TimeSlabHead { get; set; }

        public int SecondTimeSlabId { get; set; }
        public string SecondTimeSlabHead { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
