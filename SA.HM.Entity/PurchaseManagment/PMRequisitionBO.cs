using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMRequisitionBO : PMRequisitionDetailsBO
    {
        //public int RequisitionId { get; set; }
        public string PRNumber { get; set; }
        public int FromCostCenterId { get; set; }
        public int FromLocationId { get; set; }
        public int ToCostCenterId { get; set; }
        public int ToLocationId { get; set; }
        public DateTime ReceivedByDate { get; set; }
        //public string RequisitionBy { get; set; }
        //public string ApprovedStatus { get; set; }
        public string Remarks { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string FromCostCenter { get; set; }
        public string ToCostCenter { get; set; }
        public string CreatedByName { get; set; }
        public string POStatus { get; set; }
        public string ReceiveStatus { get; set; }
        public string TransferStatus { get; set; }

        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }

        public bool? IsCanEdit { get; set; }
        public bool? IsCanDelete { get; set; }
        public int? IsCanCheckeAble { get; set; }
        public int? IsCanApproveAble { get; set; }
        

    }
}
