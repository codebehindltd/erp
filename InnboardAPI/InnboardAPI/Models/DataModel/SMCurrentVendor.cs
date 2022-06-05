namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMCurrentVendor")]
    public partial class SMCurrentVendor
    {
        [Key]
        public int CurrentVendorId { get; set; }

        [Required]
        [StringLength(250)]
        public string VendorName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(25)]
        public string ContactNo { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
