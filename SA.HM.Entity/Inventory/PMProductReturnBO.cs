using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReturnBO
    {
        public long ReturnId { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public string ReturnDateString { get; set; }
        public string ReturnNumber { get; set; }
        public string ReturnType { get; set; }
        public Int64 TransactionId { get; set; }
        public int FromCostCenterId { get; set; }
        public int FromLocationId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public System.DateTime? LastModifiedDate { get; set; }

        //add
        public decimal ReturnQuantity { get; set; }
        public string UserName { get; set; }
        public string LocationTo { get; set; }
        public long ReturnDetailsId { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; } 

        //----------
        public string IssueType { get; set; }
        public string IssueNumber { get; set; }
        public string FromCostCenter { get; set; }
        public string FromLocation { get; set; }
        public int ToCostCenterId { get; set; }
        public string CostCenterTo { get; set; }
        public string CreatedByName { get; set; }
        public string ConsumptionBy { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        
        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }

        public string SerialNo { get; set; }

    }
}
