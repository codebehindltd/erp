namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMDealStage")]
    public partial class SMDealStage
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string DealStage { get; set; }

        public decimal? Complete { get; set; }

        [StringLength(20)]
        public string ForcastType { get; set; }

        [StringLength(20)]
        public string ForcastCategory { get; set; }

        public int? DisplaySequence { get; set; }

        public string Description { get; set; }

        public bool IsSiteSurvey { get; set; }

        public bool IsQuotationReveiw { get; set; }

        public bool IsCloseWon { get; set; }

        public bool IsCloseLost { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
