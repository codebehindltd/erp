using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
   public class ActivityLogsBO
    {
        public int ActivityId { get; set; }
        public string ActivityType { get; set; }
        public string EntityType { get; set; }
        public long EntityId { get; set; }
        public string Remarks { get; set; }
        public string Module { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedByDate { get; set; }
    }
}
