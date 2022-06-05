using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.UserInformation
{
    public class UserGroupBO
    {
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int DefaultModuleId { get; set; }
        public int DefaultHomePageId { get; set; }
        public string DefaultHomePage { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string Email { get; set; }
        public string UserGroupType { get; set; }
    }
}
