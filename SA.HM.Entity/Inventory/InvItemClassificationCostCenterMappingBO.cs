using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemClassificationCostCenterMappingBO
    {
        public Int64 MappingId { get; set; }
        public Int64 CostCenterId { get; set; }
        public Int64 ClassificationId { get; set; }
        public Int64 AccountHeadId { get; set; }

        public Int64 CreatedBy { get; set; }
        public Int64 LastModifiedBy { get; set; }
    }
}
