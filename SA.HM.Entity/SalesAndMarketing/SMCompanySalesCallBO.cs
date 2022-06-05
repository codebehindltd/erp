using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCompanySalesCallBO
    {
        public Int64 SalesCallId { get; set; }
        public int CompanyId { get; set; }
        public int SiteId { get; set; }        
        public DateTime InitialDate { get; set; }
        public DateTime FollowupDate { get; set; }
        public string Remarks { get; set; }
        public int LocationId { get; set; }
        public int CityId { get; set; }
        public int IndustryId { get; set; }
        public int CITypeId { get; set; }
        public int ActionPlanId { get; set; }
        public int OpportunityStatusId { get; set; }
        public int FollowupTypeId { get; set; }    
        public string FollowupType { get; set; }
        public int PurposeId { get; set; } 
        public string Purpose { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string CompanyName { get; set; }
        public string InitialTime { get; set; }
        public string FollowupTime { get; set; }

        public string CIType { get; set; }
        public string ActionPlan { get; set; }
        public string OpportunityStatus { get; set; }
        public string LocationName { get; set; }

        public List<SMCompanySalesCallDetailBO> SMCompanySalesCallDetailList { get; set; }
    }
}
