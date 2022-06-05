using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class SecurityUserAdminAuthorizationBO
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public int ModuleId { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
