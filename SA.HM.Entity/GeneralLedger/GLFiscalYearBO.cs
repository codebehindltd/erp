using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLFiscalYearBO
    {
        public int FiscalYearId { get; set; }
        public int ProjectId { get; set; }
        public string FiscalYearName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string FromDateForClientSideShow { get; set; }
        public string TodateForClientSideShow { get; set; }
        public string ReportFromDate { get; set; }
        public string ReportToDate { get; set; }
    }
}
