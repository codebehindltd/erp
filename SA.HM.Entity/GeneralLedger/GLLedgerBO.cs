using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLLedgerBO
    {
        public int LedgerId { get; set; }
        public int GLMasterId { get; set; }
        public Int64 NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public int LedgerMode { get; set; }
        public int BankAccountId { get; set; }
        public string ChequeNumber { get; set; }
        public decimal LedgerAmount { get; set; }
        public decimal LedgerDebitAmount { get; set; }
        public decimal LedgerCreditAmount { get; set; }
        public string NodeNarration { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public int FieldId { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public int VoucherMode { get; set; }
    }
}
