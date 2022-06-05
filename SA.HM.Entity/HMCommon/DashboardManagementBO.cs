using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DashboardManagementBO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long UserGroupId { get; set; }
        public long ItemId { get; set; }
        public int Panel { get; set; }
        public string DivName { get; set; }                     
    }
}
