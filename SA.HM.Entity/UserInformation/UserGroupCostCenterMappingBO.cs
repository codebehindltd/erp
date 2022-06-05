using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.UserInformation
{
    public class UserGroupCostCenterMappingBO
    {
        public int MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int ProjectId { get; set; }
        public int UserGroupId { get; set; }
    }
}
