using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DayCloseBO
    {
        public int DayCloseId { get; set; }
        public string DayCloseDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int DataCount { get; set; }
        public string DayClosedDescription { get; set; }

        public long Id { get; set; }
        public DateTime ProcessFromDate { get; set; }
        public DateTime ProcessToDate { get; set; }
    }
}
