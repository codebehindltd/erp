using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class ModuleNameBO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string GroupName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public Boolean IsReportType { get; set; }
        public int ObjectTabId { get; set; }
        public string MenuHead { get; set; }
        public int MenuLinksId { get; set; }
        public string PageName { get; set; }
        public string PageDisplayCaption { get; set; }
    }
}
