using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class HotelGuestCompany
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string ContactDesignation { get; set; }
        public string TelephoneNumber { get; set; }
        public string Remarks { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? CompanyOwnerId { get; set; }
        public int? IndustryId { get; set; }
        public int? LocationId { get; set; }
        public int? NodeId { get; set; }
        public int? SignupStatusId { get; set; }
        public DateTime? AffiliatedDate { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool? IsMember { get; set; }
        public decimal? Balance { get; set; }
        public int? NumberOfEmployee { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public long? DealStageWiseCompanyStatusId { get; set; }
        public bool IsClient { get; set; }
        public int? AncestorId { get; set; }
        public int? Lvl { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public int? CompanyType { get; set; }
        public int? OwnershipId { get; set; }
        public string TicketNumber { get; set; }
        public int? LifeCycleStageId { get; set; }
        public string BillingStreet { get; set; }
        public int? BillingCityId { get; set; }
        public int? BillingStateId { get; set; }
        public int? BillingCountryId { get; set; }
        public string BillingPostCode { get; set; }
        public string ShippingStreet { get; set; }
        public int? ShippingCityId { get; set; }
        public int? ShippingStateId { get; set; }
        public int? ShippingCountryId { get; set; }
        public string ShippingPostCode { get; set; }
        public string Fax { get; set; }
        public string CompanyNumber { get; set; }
    }
}
