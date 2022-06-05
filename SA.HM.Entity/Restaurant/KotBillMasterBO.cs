using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class KotBillMasterBO
    {
        public int KotId { get; set; }
        public string TokenNumber { get; set; }
        public DateTime KotDate { get; set; }
        public int BearerId { get; set; }
        public string BearerName { get; set; }
        public string WaiterName { get; set; }
        public int CostCenterId { get; set; }
        public string SourceName { get; set; }
        public int SourceId { get; set; }
        public string SourceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaxQuantity { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string Remarks { get; set; }
        public int RegistrationId { get; set; }
        public int RoomId { get; set; }
        public string KotStatus { get; set; }
        public bool IsBillProcessed { get; set; }
        public bool IsBillHoldup { get; set; }
        public int BillId { get; set; }
        public string CategoryList { get; set; }
        public bool IsStopChargePosting { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? KotDateForAPI { get; set; }
        public bool? IsKotReturn { get; set; }
        public int? ReferenceKotId { get; set; }
        public string CostCenter { get; set; }
        public string TransactionType { get; set; }
        public string TransactionBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDateDisplay { get; set; }

    }
}
