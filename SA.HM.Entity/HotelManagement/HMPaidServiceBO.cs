using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HMPaidServiceBO
    {
        public int PaidServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
        public int CostCenterId { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
