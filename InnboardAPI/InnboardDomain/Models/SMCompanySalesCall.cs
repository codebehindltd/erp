namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMCompanySalesCall")]
    public partial class SMCompanySalesCall
    {
        [Key]
        public long SalesCallId { get; set; }

        public long? CompanyId { get; set; }

        public int? SiteId { get; set; }

        public DateTime? InitialDate { get; set; }

        public DateTime? FollowupDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? FollowupTypeId { get; set; }

        [StringLength(250)]
        public string FollowupType { get; set; }

        public int? PurposeId { get; set; }

        [StringLength(250)]
        public string Purpose { get; set; }

        public int? LocationId { get; set; }

        public int? CityId { get; set; }

        public int? IndustryId { get; set; }

        public int? CITypeId { get; set; }

        public int? ActionPlanId { get; set; }

        public int? OpportunityStatusId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
