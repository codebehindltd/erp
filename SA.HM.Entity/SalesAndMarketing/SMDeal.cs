using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMDeal
    {
        public long Id { get; set; }
        public long RandomDealId { get; set; }
        public string Owner { get; set; }
        public int? OwnerId { get; set; }
        public string Company { get; set; }
        public int? CompanyId { get; set; }
        public string DealNumber { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ExpectedRevenue { get; set; }
        public string Type { get; set; }
        public string Stage { get; set; }
        public int? StageId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        
        public int? ProbabilityStageId { get; set; }
        public string ProbabilityStage { get; set; } 
        public int? SegmentId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual List<SMDealWiseContactMap> Contacts { get; set; }

        public long QuotationId { get; set; }
        public bool IsQuotationReview { get; set; }
        public bool IsSiteSurvey { get; set; }
        public long SiteSurveyNoteId { get; set; }
        public bool IsCloseWon { get; set; }
        public bool Is { get; set; }
        public bool IsCanDelete { get; set; }
        public long ContactId { get; set; }
        public string ContactName { get; set; }

        public DateTime? ImplementationDate { get; set; }
        public string ImplementationFeedback { get; set; }
        public string ImplementationStatus { get; set; }
        public int GLCompanyId { get; set; }
        public int GLProjectId { get; set; }

    }
}
