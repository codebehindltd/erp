namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelSegmentRateChart")]
    public partial class HotelSegmentRateChart
    {
        [Key]
        public int RateChartId { get; set; }

        public int? SegmentId { get; set; }

        public int? RoomTypeId { get; set; }

        public decimal? RoomRate { get; set; }

        public decimal? RoomRateUSD { get; set; }

        public DateTime? EffectiveFromDate { get; set; }

        public DateTime? EffectiveToDate { get; set; }
    }
}
