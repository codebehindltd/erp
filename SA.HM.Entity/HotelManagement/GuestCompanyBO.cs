using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestCompanyBO
    {
        public int CompanyId { get; set; }
        public long RandomProductId { get; set; }       
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddressWithoutLabel { get; set; }
        public string WebAddress { get; set; }
        public string ContactNumber { get; set; }
        public string ContactNumberWithoutLabel { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string TelephoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Remarks { get; set; }
        public string SignupStatus { get; set; }
        public DateTime? AffiliatedDate { get; set; }
        public int? CompanyOwnerId { get; set; }
        public string CompanyOwnerName { get; set; }
        public int? LocationId { get; set; }        
        public string IndustryName { get; set; }
        public decimal DiscountPercent { get; set; }
        public string ContactDesignation { get; set; }
        public bool IsMember { get; set; }
        public string FirstInitialDateString { get; set; }
        public string FirstInitialTimeString { get; set; }
        public string LastFollowUpDateString { get; set; }
        public string LastFollowUpTimeString { get; set; }
        public int? CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string CreatedDisplayDate { get; set; }
        public int NodeId { get; set; }
        public decimal Balance { get; set; }
        public string GroupName { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime CreditLimitExpireDate { get; set; }
        public decimal ShortCreditLimit { get; set; }
        public DateTime? ShortCreditLimitExpireDate { get; set; }
        public decimal TransportFarePerKgFactory { get; set; }
        public decimal TransportFarePerKgDepot { get; set; }
        public decimal CommissionPerKg { get; set; }
        public string BillingType { get; set; }
        public string LegalAction { get; set; }
        public string LegalActionDetails { get; set; }
        public int? SignupStatusId { get; set; }        
        public long? DealStageWiseCompanyStatusId { get; set; }
        public string DealStageWiseCompanyStatus { get; set; }
        public bool IsClient { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }
        public int? NumberOfEmployee { get; set; }
        public decimal? AnnualRevenue { get; set; }
        //new add 
        public int? AncestorId { get; set; }
        public int? Lvl { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public int? CompanyType { get; set; }
        public string TypeName { get; set; }
        public string OwnershipName { get; set; }
        public string MatrixInfo { get; set; }        
        public int? IndustryId { get; set; }
        public int? OwnershipId { get; set; }
        public string TicketNumber { get; set; }
        public Int64? LifeCycleStageId { get; set; }
        public string LifeCycleStage { get; set; }
        public int? BillingLocationId { get; set; }
        public string BillingStreet { get; set; }
        public int? BillingCityId { get; set; }
        public int? BillingStateId { get; set; }
        public int? BillingCountryId { get; set; }
        public string BillingPostCode { get; set; }
        public int? ShippingLocationId { get; set; }
        public string ShippingStreet { get; set; }
        public int? ShippingCityId { get; set; }
        public int? ShippingStateId { get; set; }
        public int? ShippingCountryId { get; set; }
        public string ShippingPostCode { get; set; }
        public string BillingCountry { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string CompanyNameWithCode { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int ParentCompanyId { get; set; }
        public string ParentCompany { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyEmail { get; set; }
        public string CountryName { get; set; }
        public string AssociateContacts { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string AccountManager { get; set; }
        public int IsDetailPanelEnableForCompany { get; set; }
        public int IsDetailPanelEnableForParentCompany { get; set; }
    }
}
