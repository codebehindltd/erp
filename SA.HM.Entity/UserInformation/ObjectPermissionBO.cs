using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.UserInformation
{
    public class ObjectPermissionBO
    {
        public int ObjectPermissionId { get; set; }
        public int ObjectTabId { get; set; }
        public string ObjectGroupHead { get; set; }
        public string ObjectHead { get; set; }
        public string MenuHead { get; set; }
        public string ObjectType { get; set; }
        public int UserGroupId { get; set; }
        public Boolean IsSavePermission { get; set; }
        public string SaveStatus { get; set; }
        public Boolean IsUpdatePermission { get; set; }
        public string UpdateStatus { get; set; }
        public Boolean IsDeletePermission { get; set; }
        public string DeleteStatus { get; set; }
        public Boolean IsViewPermission { get; set; }
        public string ViewStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
