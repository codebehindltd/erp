using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ServiceBandwidthBO
    {
        public int ServiceBandWidthId { get; set; }
        public string BandWidthName { get; set; }
        public int? Uplink { get; set; }
        public string UplinkFrequency { get; set; }
        public bool? ActiveStat { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DownlinkFrequency { get; set; }
        public string Description { get; set; }
        public int? Downlink { get; set; }
        //public int BandWidthValue { get; set; }
        public string ActiveStatus { get; set; }
        // new add
        //public string Description { get; set; }
        //public int Uplink { get; set; }
        //public string UplinkFrequency { get; set; }
        //public int Downlink { get; set; }
        //public string DownlinkFrequency { get; set; }
    }
}
