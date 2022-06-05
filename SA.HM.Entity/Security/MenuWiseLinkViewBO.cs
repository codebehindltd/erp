using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Security
{
    public class MenuWiseLinkViewBO : MenuWiseLinksBO
    {
        public int GroupDisplaySequence { get; set; }
        public string MenuGroupName { get; set; }
        public int LinksDisplaySequence { get; set; }
        public string PageId { get; set; }
        public string PageExtension { get; set; }
        public string PagePath { get; set; }
        public string PageType { get; set; }
        public string PageName { get; set; }
        public int ModuleId { get; set; }
        public string GroupName { get; set; }
    }
}
