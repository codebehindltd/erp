using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpTransferReportViewBO
    {
        public string DisplayName { get; set; }
        public string PreviousDepartment { get; set; }
        public string CurrentDepartment { get; set; }
        public string ReportingTo { get; set; }
        public string PreviousReportingTo { get; set; }
        public string CurrentCompanyName { get; set; }
        public string PreviousCompanyName { get; set; }
        public string PreviousDesignation { get; set; }
        public string CurrentDesignation { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime ReportingDate { get; set; }
    }
}
