using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelGuestServiceInfoBO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
        public int CostCenterId { get; set; }
        public int AccountHeadId { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public bool IsVatEnable { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsCitySDChargeEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }
        public bool IsGeneralService { get; set; }
        public bool IsPaidService { get; set; }
        public bool IsNextDayAchievement { get; set; }

        public string VatEnable { get; set; }
        public string ServiceChargeEnable { get; set; }
        public string GeneralService { get; set; }
        public string PaidService { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
