using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLMasterBO
    {
        public int GLMasterId { get; set; }
        public int CostCentreId { get; set; }
        public int VoucherMode { get; set; }
        public int TransactionMode { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Narration { get; set; }
        public string PayerOrPayee { get; set; }
        //public int CashChequeMode { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
