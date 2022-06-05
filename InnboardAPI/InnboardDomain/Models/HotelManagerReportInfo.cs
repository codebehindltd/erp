namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelManagerReportInfo")]
    public partial class HotelManagerReportInfo
    {
        [Key]
        public long SummaryId { get; set; }

        public DateTime? SummaryDate { get; set; }

        public int? OrderByNumber { get; set; }

        public string ServiceType { get; set; }

        public string ServiceName { get; set; }

        public decimal? Covers { get; set; }
    }
}
