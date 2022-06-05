using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ReceivablePayableReportViewBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int? BSGroupId { get; set; }
        public string BSGroupHead { get; set; }
        public int? NodeGroupId { get; set; }
        public string NodeGroupHead { get; set; }
        public int? HeadId { get; set; }
        public string NodeHead { get; set; }
        public decimal? Amount { get; set; }
        public string NotesNumber { get; set; }
    }
}
