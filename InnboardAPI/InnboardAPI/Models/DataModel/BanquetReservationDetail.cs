namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetReservationDetail")]
    public partial class BanquetReservationDetail
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        public long? ItemTypeId { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; }

        public long? ItemId { get; set; }

        [StringLength(300)]
        public string ItemName { get; set; }

        public bool? IsComplementary { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public virtual BanquetReservation BanquetReservation { get; set; }
    }
}
