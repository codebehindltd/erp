namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMLifeCycleStage")]
    public partial class SMLifeCycleStage
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string LifeCycleStage { get; set; }

        public int? DisplaySequence { get; set; }

        public string Description { get; set; }

        public bool IsRelatedToDeal { get; set; }

        [StringLength(20)]
        public string DealType { get; set; }

        [StringLength(20)]
        public string ForcastType { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
