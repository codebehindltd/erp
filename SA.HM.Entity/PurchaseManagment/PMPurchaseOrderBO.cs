using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMPurchaseOrderBO
    {
        public int POrderId { get; set; }
        public int CompanyId { get; set; }
        public DateTime PODate { get; set; }
        public string PODateDisplay { get; set; }
        public DateTime? ReceivedByDate { get; set; }
        public string PONumber { get; set; }
        public string POType { get; set; }
        public string PRNumber { get; set; }
        public string IsLocalOrForeignPO { get; set; }
        public int SupplierId { get; set; }
        public string OrderType { get; set; }
        public string SupplierName { get; set; }
        public string ApprovedStatus { get; set; }
        public string Remarks { get; set; }
        public string CostCenter { get; set; }
        public int CostCenterId { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public List<PMPurchaseOrderDetailsBO> OrderDetails { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string ReceiveStatus { get; set; }
        public int PurchaseOrderTemplate { get; set; }
        
        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
        public bool IsCanPOReOpen { get; set; }
        public string DeliveryAddress { get; set; }
        public string PODescription { get; set; }
        public int LocalCurrencyId { get; set; }
        public string LocalCurrencyName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyType { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal PurchaseOrderAmount { get; set; }        
        public List<PMPurchaseOrderTermsNConditionBO> TermsNConditions { get; set; }
        public string DeliveryStatus { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public string StockByHead { get; set; }
        public string DeliveryDateDisplay { get; set; }
    }
}
