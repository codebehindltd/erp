using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLDealMasterBO
    {
        public long DealId { get; set; }
        public int ProjectId { get; set; }
        public int VoucherMode { get; set; }
        public string VoucherType { get; set; }
        public int BankExistOrNot { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Narration { get; set; }
        public string ReferenceNumber { get; set; }
        public string PayerOrPayee { get; set; }
        public int CashChequeMode { get; set; }
        public string GLStatus { get; set; }
        public int CompanyId { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }

        public string NodeHead { get; set; }
        public string ChequeNumber { get; set; }
        public decimal LedgerAmount { get; set; }

        public int CheckedBy { get; set; }
        public int ApprovedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
    }
}
