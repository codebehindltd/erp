using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveTypeBO
    {
        public int LeaveTypeId { get; set; }
        public int LeaveModeId { get; set; }
        public string LeaveMode { get; set; }
        public string TypeName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int YearlyLeave { get; set; }
        public bool CanCarryForward { get; set; }
        public byte MaxDayCanCarryForwardYearly { get; set; }
        public byte MaxDayCanKeepAsCarryForwardLeave { get; set; }
        public bool CanEncash { get; set; }
        public byte MaxDayCanEncash { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
