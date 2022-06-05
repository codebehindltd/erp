using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DashboardItemBO
    {
        public long Id { get; set; }
        public long UserMappingId { get; set; }
        public int ModuleId { get; set; }
        public string ItemName { get; set; }
        
        public string ActiveStatus { get; set; }
    }
}
