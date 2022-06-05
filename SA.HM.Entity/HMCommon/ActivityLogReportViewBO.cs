using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class ActivityLogReportViewBO
    {
        public string ActivityType { get; set; }
        public string UserName { get; set; }
        public string Module { get; set; }
        public string Remarks { get; set; }
        public string FieldName { get; set; }
        public string PreviousData { get; set; }
        public string CurrentData { get; set; }
        public string CreatedByDate { get; set; }
    }
}
