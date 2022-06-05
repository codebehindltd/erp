using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSiteSurveyNoteBO
    {
        public long Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public Nullable<long> DealId { get; set; }
        public string Deal { get; set; }
        public bool IsDealNeedSiteSurvey { get; set; }
        public bool IsSiteSurveyUnderCompany { get; set; }
        public string Description { get; set; }
        public Nullable<long> SegmentId { get; set; }
        public string Segment { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
