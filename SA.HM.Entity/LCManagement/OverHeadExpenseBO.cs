using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class OverHeadExpenseBO
    {
        public int ExpenseId { get; set; }
        public int LCId { get; set; }
        public string LCNumber { get; set; }
        public string TransactionType { get; set; }
        public int OverHeadId { get; set; }
        public int CNFId { get; set; }
        public string OverHeadName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal LocalCurrencyAmount { get; set; }
        public string Description { get; set; }
        public string TransactionNo { get; set; }
        public string Status { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeNumber { get; set; }
        public int? BankId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? ConversionRate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int IsDayClosed { get; set; }
        public int TransactionAccountHeadId { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
        public string ExpenseDateDisplay { get; set; }
        public string AccountingPostingHead { get; set; }
        public string CurrencyType { get; set; }
        public string ChecqueDateDisplay { get; set; }
        public string CreatedByName { get; set; }
    }
}
