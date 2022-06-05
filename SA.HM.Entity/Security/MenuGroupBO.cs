
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Security
{
    public class MenuGroupBO
    {
        public long MenuGroupId { get; set; }
        public string MenuGroupName { get; set; }
        public string GroupDisplayCaption { get; set; }
        public int DisplaySequence { get; set; }
        public string GroupIconClass { get; set; }
        public bool ActiveStat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public string GroupName { get; set; }
    }
}
