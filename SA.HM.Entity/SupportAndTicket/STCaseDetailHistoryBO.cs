using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class STCaseDetailHistoryBO
    {
        public long Id { get; set; }
        public long CaseId { get; set; }
        public string ShortInternalNotesDetails { get; set; }
        public string InternalNotesDetails { get; set; }   
        public int LogNumber { get; set; }
    }
}
