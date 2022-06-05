using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReceivedBO
    {
        public int ReceivedId { get; set; }
        public string ReceiveType { get; set; }
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }
        public int POrderId { get; set; }
        public string OrderCode { get; set; }
        public string ReceiveNumber { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string ReferenceNumber { get; set; }
        public string PurchaseBy { get; set; }

        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public int SupplierId { get; set; }
        public string PONumber { get; set; }
        public decimal PurchasePrice { get; set; }
        public int ReceivedProductTemplate { get; set; }

        public string CostCenter { get; set; }
        public string SupplierName { get; set; }
        public string LocationName { get; set; }
        public int CostCenterId { get; set; }
        public string Remarks { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ConvertionRate { get; set; }
        public DateTime? ReferenceBillDate { get; set; }

        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }

        public string ReceiveStatus { get; set; }

    }
}
