using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelRepairNMaintenanceBO
    {
        public long Id { get; set; }
        public string MaintenanceType { get; set; }
        public string ItemName { get; set; }
        public string FixedItemName { get; set; }
        public int? ItemId { get; set; }
        public string Details { get; set; }
        public string MaintenanceArea { get; set; }
        public int? TransectionId { get; set; }
        public bool IsEmergency { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ExpectedTime { get; set; }
        public int RequestedById { get; set; }
        public string RequestedByName { get; set; }
        public string RepairNMaintenanceNo { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
