using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CostCentreTabBO
    {
        public int CostCenterId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public int GLCompanyId { get; set; }
        public string CompanyType { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterLogo { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal CitySDCharge { get; set; }
        public bool IsVatOnSDCharge { get; set; }
        public bool IsCitySDChargeEnableOnServiceCharge { get; set; }
        public string AdditionalChargeType { get; set; }
        public decimal AdditionalCharge { get; set; }
        public int OutletType { get; set; }
        public int IsVatSChargeInclusive { get; set; }
        public int IsRatePlusPlus { get; set; }
        public bool IsDiscountApplicableOnRackRate { get; set; }
        public string CostCenterType { get; set; }
        public Boolean IsRestaurant { get; set; }
        public Boolean IsDiscountEnable { get; set; }
        public Boolean IsEnableItemAutoDeductFromStore { get; set; }
        public string DefaultView { get; set; }
        public int DefaultStockLocationId { get; set; }
        public int InvoiceTemplate { get; set; }
        public int BillingStartTime { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDefaultCostCenter { get; set; }
        public int MappingId { get; set; }
        public int StopChargePostingStatus { get; set; }
        public string BillNumberPrefix { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsCitySDChargeEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }
        public bool IsVatEnable { get; set; }
        public int Index { get; set; }
        public int PayrollDeptId { get; set; }
        public Boolean IsCostCenterNameShowOnInvoice { get; set; }
        public Boolean IsCustomerDetailsEnable { get; set; }
        public Boolean IsDeliveredByEnable { get; set; }
        public string VatRegistrationNo { get; set; }
        public string ContactNumber { get; set; }
    }
}
