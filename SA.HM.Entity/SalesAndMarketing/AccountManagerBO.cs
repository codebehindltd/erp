using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class AccountManagerBO
    {
        public int AccountManagerId { get; set; }
        public int UserInfoId { get; set; }
        public int EmpId { get; set; }
        public int DepartmentId { get; set; }
        public string SortName { get; set; }
        public string OfficialEmail { get; set; }
        public int? AncestorId { get; set; }
        public int Lvl { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string DisplayName { get; set; }
        public string AccountManager { get; set; }
        public string SupervisonName { get; set; }
    }
}
