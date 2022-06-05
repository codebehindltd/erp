namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemClassificationCostCenterMapping")]
    public partial class InvItemClassificationCostCenterMapping
    {
        [Key]
        public long MappingId { get; set; }

        public long? CostCenterId { get; set; }

        public long? ClassificationId { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        public virtual InvItemClassification InvItemClassification { get; set; }
    }
}
