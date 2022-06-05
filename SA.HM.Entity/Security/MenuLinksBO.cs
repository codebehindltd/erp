using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Security
{
    public class MenuLinksBO
    {
        public long MenuLinksId { get; set; }
        public int ModuleId { get; set; }
        public string PageId { get; set; }
        public string PageName { get; set; }
        public string PageDisplayCaption { get; set; }
        public string PageExtension { get; set; }
        public string PagePath { get; set; }
        public string PageType { get; set; }
        public string LinkIconClass { get; set; }
        public bool ActiveStat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
