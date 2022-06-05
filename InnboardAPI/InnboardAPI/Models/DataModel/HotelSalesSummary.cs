namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelSalesSummary")]
    public partial class HotelSalesSummary
    {
        [Key]
        public long SummaryId { get; set; }

        public DateTime? SummaryDate { get; set; }

        [StringLength(200)]
        public string ServiceType { get; set; }

        [StringLength(200)]
        public string ServiceName { get; set; }

        public int? Covers { get; set; }

        public decimal? TotalSales { get; set; }

        public decimal? TotalVat { get; set; }

        public decimal? TotalServiceCharge { get; set; }

        public decimal? TotalCitySDCharge { get; set; }

        public decimal? TotalAdditionalCharge { get; set; }

        public decimal? TotalRoomSale { get; set; }

        public decimal? TotalRoomVat { get; set; }

        public decimal? TotalRoomServiceCharge { get; set; }

        public decimal? TotalRoomCitySDCharge { get; set; }

        public decimal? TotalRoomAdditionalCharge { get; set; }

        public decimal? RoomOccupied { get; set; }

        public decimal? OccupencyPercent { get; set; }

        public decimal? DoubleOccupency { get; set; }

        public decimal? NoOfGuest { get; set; }

        public int PaxQuantity { get; set; }
    }
}
