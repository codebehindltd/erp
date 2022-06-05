using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductOutBO
    {
        public int OutId { get; set; }
        public string ProductOutFor { get; set; }
        public string TransferFor { get; set; }
        public DateTime OutDate { get; set; }
        public string StringReturnDate { get; set; }
        public int RequisitionOrSalesId { get; set; }
        public int? OutFor { get; set; }
        public string IssueType { get; set; }
        public string BillNo { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string RequisitionOrSalesNumber { get; set; }
        public string DelivaredStatus { get; set; }
        public int ReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ReceivedId { get; set; }
        public int POrderId { get; set; }
        public string PONumber { get; set; }
        public string IssueNumber { get; set; }
        public Int64? AccountPostingHeadId { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Int32? FromCostCenterId { get; set; }
        public Int32? FromLocationId { get; set; }
        public Int32? ToCostCenterId { get; set; }
        public Int32? ToLocationId { get; set; }
        public Int32? EmployeeId { get; set; }
        public string FromCostCenter { get; set; }
        public string ToCostCenter { get; set; }
        public string FromLocationFrom { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public string PRNumber { get; set; }
        public string DeliveryStatus { get; set; }
        public string CreatedByName { get; set; }
        public int GLCompanyId { get; set; }
        public int GLProjectId { get; set; }
        public int ToGLCompanyId { get; set; }
        public int ToGLProjectId { get; set; }
        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
