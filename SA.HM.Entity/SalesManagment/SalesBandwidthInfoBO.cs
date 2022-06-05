using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class SalesBandwidthInfoBO
    {
        public int BandwidthInfoId { get; set; }
        public string BandwidthType { get; set; }
        public string BandwidthName { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
