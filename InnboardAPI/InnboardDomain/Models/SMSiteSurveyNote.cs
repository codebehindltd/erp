namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSiteSurveyNote")]
    public partial class SMSiteSurveyNote
    {
        public long Id { get; set; }

        public bool IsSiteSurveyUnderCompany { get; set; }

        public int? CompanyId { get; set; }

        public int? ContactId { get; set; }

        public string Address { get; set; }

        public long? DealId { get; set; }

        public bool IsDealNeedSiteSurvey { get; set; }

        public string Description { get; set; }

        public long? SegmentId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Status { get; set; }
    }
}
