using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ServicePriceMatrixBO
    {
        public int ServicePriceMatrixId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ServicePackageId { get; set; }
        public int ServiceBandWidthId { get; set; }     
        public decimal UnitPrice { get; set; }
        public Boolean IsActive { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string Description { get; set; }
        public string PackageName { get; set; }
        public int UplinkFrequencyId { get; set; }
        public int UplinkFrequency { get; set; }
        public string UplinkFrequencyUnit { get; set; }
        public int DownlinkFrequency { get; set; }
        public int DownlinkFrequencyId { get; set; }
        public string DownlinkFrequencyUnit { get; set; }
        public string ShareRatio { get; set; }
    }
}
