namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMDeal")]
    public partial class SMDeal
    {
        public long Id { get; set; }

        public int? OwnerId { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public decimal? Amount { get; set; }

        public decimal? ExpectedRevenue { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? StageId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public int? ProbabilityStageId { get; set; }

        public int? SegmentId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public DateTime? ImplementationDate { get; set; }

        [StringLength(200)]
        public string ImplementationFeedback { get; set; }

        public virtual HotelGuestCompany HotelGuestCompany { get; set; }
    }
}
