using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTransferBO
    {
        public long TransferId { get; set; }
        public DateTime TransferDate { get; set; }
        public long EmpId { get; set; }
        public int PreviousDepartmentId { get; set; }
        public int CurrentDepartmentId { get; set; }
        public int PreviousDesignationId { get; set; }
        public int CurrentDesignationId { get; set; }
        public int? PreviousLocation { get; set; }
        public int? CurrentLocation { get; set; }
        public DateTime ReportingDate { get; set; }
        public DateTime JoinedDate { get; set; }
        public long ReportingToId { get; set; }
        public long ReportingTo2Id { get; set; }
        public string ApprovedStatus { get; set; }

        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string PreviousDepartmentName { get; set; }
        public string CurrentDepartmentName { get; set; }
        public string PreviousDesignationName { get; set; }
        public string CurrentDesignationName { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public long PreviousReportingToId { get; set; }
        public string PreviousReportingToName { get; set; }
        public string PreviousReportingTo2Name { get; set; }
        public long PreviousReportingTo2Id { get; set; }
        public long PreviousCompanyId { get; set; }
        public string PreviousCompanyName { get; set; }
        public long CurrentCompanyId { get; set; }
        public string CurrentCompanyName { get; set; }
        public long PreviousProjectId { get; set; }
        public string PreviousProjectName { get; set; }
        public long CurrentProjectId { get; set; }
        public string CurrentProjectName { get; set; } 
        public string ReportingTo { get; set; }
        public string Description { get; set; }
        public DateTime LastModifiedDate { get; set; }



    }
}
