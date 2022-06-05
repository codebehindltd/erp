using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class RosterHeadBO
    {
        public int RosterId { get; set; }
        public string RosterName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
