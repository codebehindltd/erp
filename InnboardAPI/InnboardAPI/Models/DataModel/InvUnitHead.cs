namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvUnitHead")]
    public partial class InvUnitHead
    {
        [Key]
        public int UnitHeadId { get; set; }

        [StringLength(100)]
        public string HeadName { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
