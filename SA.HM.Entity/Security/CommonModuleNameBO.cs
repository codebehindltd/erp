
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Security
{
    public class CommonModuleNameBO
    {
        public int ModuleId { get; set; }
        public int TypeId { get; set; }
        public string ModuleName { get; set; }
        public string GroupName { get; set; }
        public string ModulePath { get; set; }
        public bool IsReportType { get; set; }
        public bool ActiveStat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
