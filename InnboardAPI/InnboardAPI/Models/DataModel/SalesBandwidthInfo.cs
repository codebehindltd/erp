namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesBandwidthInfo")]
    public partial class SalesBandwidthInfo
    {
        [Key]
        public int BandwidthInfoId { get; set; }

        [StringLength(50)]
        public string BandwidthType { get; set; }

        [StringLength(250)]
        public string BandwidthName { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
