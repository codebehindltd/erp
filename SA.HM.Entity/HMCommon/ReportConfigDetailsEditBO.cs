using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class ReportConfigDetailsEditBO
    {
        public long ReportConfigId { get; set; }
        public int ReportTypeId { get; set; }
        public long Id { get; set; }
        public long NodeId { get; set; }
        public string NodeName { get; set; }
        public short SortingOrder { get; set; }
        public string NodeType { get; set; }
    }
}
