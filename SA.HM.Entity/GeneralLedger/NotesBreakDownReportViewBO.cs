using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class NotesBreakDownReportViewBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string NotesName { get; set; }
        public int? DealId { get; set; }
        public Int32 NodeId { get; set; }
        public string NodeHead { get; set; }
        public string NodeNumber { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
    }
}
