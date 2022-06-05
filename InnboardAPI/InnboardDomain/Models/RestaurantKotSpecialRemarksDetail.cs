namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantKotSpecialRemarksDetail")]
    public partial class RestaurantKotSpecialRemarksDetail
    {
        [Key]
        public int RemarksDetailId { get; set; }

        public int? KotId { get; set; }

        public int? ItemId { get; set; }

        public int? SpecialRemarksId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
