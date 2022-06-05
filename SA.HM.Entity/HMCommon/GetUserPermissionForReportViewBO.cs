using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class GetUserPermissionForReportViewBO
    {
        public string GroupName { get; set; }
        public string GroupDisplayCaption { get; set; }
        public string ModuleName { get; set; }
        public string PageDisplayCaption { get; set; }
        public long MenuWiseLinksId { get; set; }
        public int UserGroupId { get; set; }
        public long MenuGroupId { get; set; }
        public long MenuLinksId { get; set; }
        public int DisplaySequence { get; set; }
        public bool IsSavePermission { get; set; }
        public Nullable<bool> IsUpdatePermission { get; set; }
        public bool IsDeletePermission { get; set; }
        public bool IsViewPermission { get; set; }
        public bool ActiveStat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
