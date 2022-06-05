namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMDealProbabilityStageInformation")]
    public partial class SMDealProbabilityStageInformation
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string ProbabilityStage { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
