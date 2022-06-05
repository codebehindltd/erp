namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelSegmentHead")]
    public partial class HotelSegmentHead
    {
        [Key]
        public int SegmentId { get; set; }

        [StringLength(200)]
        public string SegmentName { get; set; }

        [StringLength(50)]
        public string SegmentType { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
