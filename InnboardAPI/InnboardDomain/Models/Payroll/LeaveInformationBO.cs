using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models.Payroll
{
    public class LeaveInformationBO
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string LeaveMode { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveTypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfDays { get; set; }

        public string Reason { get; set; }
        public int ReportingTo { get; set; }
        public int WorkHandover { get; set; }
        public string WorkHandoverStatus { get; set; }

        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string ReportingToDesignation { get; set; }

        public string CreatedDateString { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public string TransactionType { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int Taken { get; set; }
        public int Available { get; set; }
        public string Month { get; set; }
        public int CommulativeLeave { get; set; }

        public string ShowFromDate { get; set; }
        public string ShowToDate { get; set; }


        public DateTime? JoinDate { get; set; }
        public int? YearlyLeave { get; set; }
        public int? MonthId { get; set; }
        public string LeaveMonth { get; set; }
        public int? LeaveTaken { get; set; }
        public int? TotalLeaveTaken { get; set; }
        public int? LeaveBalance { get; set; }

        public decimal OpeningLeave { get; set; }

        public int LeaveModeId { get; set; }

        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string LeaveStatus { get; set; }
        public string CancelReason { get; set; }
    }
}
