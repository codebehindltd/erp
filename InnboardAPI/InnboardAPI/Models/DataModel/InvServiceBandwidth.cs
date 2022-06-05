namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvServiceBandwidth")]
    public partial class InvServiceBandwidth
    {
        [Key]
        public int ServiceBandWidthId { get; set; }

        [Required]
        [StringLength(250)]
        public string BandWidthName { get; set; }

        public int? BandWidthValue { get; set; }

        [StringLength(25)]
        public string BandWidth { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
