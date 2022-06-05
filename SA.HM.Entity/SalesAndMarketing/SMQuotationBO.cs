using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationBO
    {
        public Int64 QuotationId { get; set; }
        public string QuotationNo { get; set; }
        public int CompanyId { get; set; }
        public DateTime ProposalDate { get; set; }
        public int ServiceTypeId { get; set; }
        public int TotalDeviceOrUser { get; set; }
        public int ContractPeriodId { get; set; }
        public int BillingPeriodId { get; set; }
        public int ItemServiceDeliveryId { get; set; }
        public int CurrentVendorId { get; set; }
        public string PriceValidity { get; set; }
        public string DeployLocation { get; set; }
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public int EmpId { get; set; }
        public string CompanyAddress { get; set; }
        public string ShowFollowupDate { get; set; }

        public Int64? DealId { get; set; }
        public Int64? ContactId { get; set; }
        public bool? IsAccepted { get; set; }
        public bool? IsRejected { get; set; }
        public bool IsSalesNoteFinal { get; set; }
        public bool IsApprovedFromBilling { get; set; }

    }
}
