using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvUnitConversionRateBO
    {
        public int ConversionId { get; set; }
        public int FromUnitHeadId { get; set; }
        public int ToUnitHeadId { get; set; }
        public decimal ConversionRate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
