using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Security
{
    public class MenuWiseLinksBO
    {
        public long MenuWiseLinksId { get; set; }
        public int MenuWiseLinksByUserInfoId { get; set; }
        public int UserGroupId { get; set; }
        public int UserId { get; set; }
        public long MenuGroupId { get; set; }
        public long MenuLinksId { get; set; }
        public int DisplaySequence { get; set; }
        public string LinkIconClass { get; set; }
        public Boolean IsSavePermission { get; set; }
        public Boolean IsUpdatePermission { get; set; }
        public string SaveStatus { get; set; }
        public Boolean IsDeletePermission { get; set; }
        public string DeleteStatus { get; set; }
        public Boolean IsViewPermission { get; set; }
        public string ViewStatus { get; set; }
        public bool ActiveStat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
