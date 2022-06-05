using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveBalanceClosingBO
    {
        public Int64 LeaveClosingId { get; set; }
        public Int64 EmpId { get; set; }        
        public int FiscalYearId { get; set; }
        public int LeaveTypeId { get; set; }                
        public decimal TakenLeave { get; set; }
        public decimal OpeningLeave { get; set; }        
        public decimal RemainingLeave { get; set; }
        public byte MaxDayCanCarryForwardYearly { get; set; }
        public byte CarryForwardedLeave { get; set; }
        public byte MaxDayCanKeepAsCarryForwardLeave { get; set; }
        public byte TotalCarryforwardLeave { get; set; }
        public byte MaxDayCanEncash { get; set; }
        public byte EncashableLeave { get; set; }
        public string  ApprovedStatus { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }        
    }
}
