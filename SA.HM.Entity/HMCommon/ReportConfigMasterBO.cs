using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class ReportConfigMasterBO
    {
        public long Id { get; set; }
        public int ReportTypeId { get; set; }
        public Nullable<long> AncestorId { get; set; }
        public string Caption { get; set; }
        public short SortingOrder { get; set; }
        public Nullable<long> Lvl { get; set; }
        public string Hierarchy { get; set; }
        public Nullable<long> HierarchyIndex { get; set; }
        public Nullable<bool> IsParent { get; set; }
        public string NodeType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
