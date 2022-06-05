using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvCategoryBO
    {
        public int CategoryId { get; set; }
        public Int32 AncestorId { get; set; }
        public string AncestorHead { get; set; }
        public int Lvl { get; set; }
        public int RandomCategoryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string HeadWithCode { get; set; }
        public string MatrixInfo { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string ImageName { get; set; }
        public int ChildCount { get; set; }

        public int CogsNodeId { get; set; }
        public int InventoryNodeId { get; set; }
        public int CostCenterId { get; set; }
    }
}
