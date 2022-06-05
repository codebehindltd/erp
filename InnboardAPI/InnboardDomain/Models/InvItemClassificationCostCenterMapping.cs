namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemClassificationCostCenterMapping")]
    public partial class InvItemClassificationCostCenterMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public long MappingId { get; set; }

        public long? CostCenterId { get; set; }

        [ForeignKey("InvItemClassification")]
        public long? ClassificationId { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        public virtual InvItemClassification InvItemClassification { get; set; }
    }
}
