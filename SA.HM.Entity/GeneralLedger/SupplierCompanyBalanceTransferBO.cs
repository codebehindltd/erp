using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class SupplierCompanyBalanceTransferBO
    {
        public long Id { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public int FromTransactionId { get; set; }
        public string FromTransactionText { get; set; }
        public int ToTransactionId { get; set; }
        public string ToTransactionText { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ApprovedStatus { get; set; }
        public int? CheckedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
