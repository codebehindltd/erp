namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelMonthToDateInfo")]
    public partial class HotelMonthToDateInfo
    {
        [Key]
        public long MTDID { get; set; }

        [Column(TypeName = "date")]
        public DateTime MTDDate { get; set; }

        public decimal? ActualRoomsOccupied { get; set; }

        public decimal? Occupency { get; set; }

        public decimal? ActualRoomsRevenue { get; set; }

        public decimal? AverageRate { get; set; }

        public decimal? RevenuePerRoom { get; set; }

        public decimal? MTDAVGRoomsOccupancy { get; set; }

        public decimal? MTDRoomsAverageRevenue { get; set; }

        public decimal? MTDAverageRate { get; set; }

        public decimal? MTDRevenuePerRoom { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
