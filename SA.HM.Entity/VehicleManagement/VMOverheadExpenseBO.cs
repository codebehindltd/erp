using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.VehicleManagement
{
    public class VMOverheadExpenseBO
    {
        public long Id { get; set; }
        public long? VehicleId { get; set; }
        public long? OverheadId { get; set; }
        public string NumberPlate { get; set; }
        public string OverheadName { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public string PaymentMode { get; set; }
        public string ChequeNumber { get; set; }
        public int? BankId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? ConversionRate { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }

        public int TransactionAccountHeadId { get; set; }

        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
