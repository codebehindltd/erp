namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestServiceInfo")]
    public partial class HotelGuestServiceInfo
    {
        [Key]
        public int ServiceId { get; set; }

        public int? NodeId { get; set; }

        public int? CostCenterId { get; set; }

        [StringLength(200)]
        public string ServiceName { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public decimal? UnitPriceUsd { get; set; }

        public bool? IsVatEnable { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public bool? IsSDChargeEnable { get; set; }

        public bool? IsAdditionalChargeEnable { get; set; }

        public bool? IsGeneralService { get; set; }

        public bool? IsPaidService { get; set; }

        public bool? IsNextDayAchievement { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
