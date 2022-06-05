using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class CNFTransactionBO
    {
        public int Id { get; set; }
        public int CNFId { get; set; }
        public string TransactionType { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ChequeNumber { get; set; }
        public int? BankId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? ConversionRate { get; set; }
        public string TransactionNo { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }

        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        //----Approval Related Attributes
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
