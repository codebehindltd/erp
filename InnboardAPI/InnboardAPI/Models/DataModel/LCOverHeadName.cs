namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCOverHeadName")]
    public partial class LCOverHeadName
    {
        [Key]
        public int OverHeadId { get; set; }

        public int? NodeId { get; set; }

        [StringLength(200)]
        public string OverHeadName { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
