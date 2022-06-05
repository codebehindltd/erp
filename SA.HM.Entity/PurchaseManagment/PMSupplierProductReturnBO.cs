using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMSupplierProductReturnBO
    {
        public Int64 ReturnId { get; set; }
        public string ReturnNumber { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ReceivedId { get; set; }
        public int? POrderId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int CreatedBy { get; set; }        
        public int LastModifiedBy { get; set; }

        public string CostCenter { get; set; }
        public string LocationName { get; set; }

        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }

    }
}
