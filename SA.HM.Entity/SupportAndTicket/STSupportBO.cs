using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class STSupportBO
    {
        public long Id { get; set; }
        public int? ClientId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameWithCode { get; set; }
        public string CaseName { get; set; }
        public string CaseNumber { get; set; }
        public int? CaseOwnerId { get; set; }        
        public int? SupportCategoryId { get; set; }
        public string SupportSource { get; set; }
        public string BillStatus { get; set; }
        public string SupportSourceOtherDetails { get; set; }
        public int? CaseId { get; set; }
        public string CaseDetails { get; set; }
        public string Feedback { get; set; }
        public string SupportStatus { get; set; }
        public string ItemOrServiceDetails { get; set; }
        public int? SupportStageId { get; set; }
        public int? CreatedBy { get; set; }
        public Int64 SerialNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? SupportDeadline { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string InternalNotesDetails { get; set; }
        public string BillConfirmation { get; set; }
        public int? SupportTypeId { get; set; }
        public int? SupportPriorityId { get; set; }
        public int? SupportForwardToId { get; set; }
        public long? TaskId { get; set; }
        public string TaskStatus { get; set; }
        public bool IsCompleted { get; set; }
        public List<STSupportDetailsBO> STSupportDetails = new List<STSupportDetailsBO>();
        public string SupportCategory { get; set; }
        public string SupportType { get; set; }
        public string FeedbackStatus { get; set; }
        public string FeedbackDetails { get; set; }
        public string SupportPriority { get; set; }
        public string SupportStage { get; set; }
        public string Department { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string SupportDeadlineDisplay { get; set; }
        public string ClientDetails { get; set; }
        public string CaseOwnerName { get; set; }
        public string CreatedByName { get; set; }
        public string LastModifiedByName { get; set; }
        public string LastModifiedDateDisplay { get; set; }
        public string AssignedTo { get; set; }
        public string CaseCloseByName { get; set; }
        public string CaseCloseDateDisplay { get; set; }
        public int PassDay { get; set; }
    }
}
