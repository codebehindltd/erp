using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLVoucherVwBO
    {
        public GLLedgerMasterVwBO LedgerMaster { get; set; }
        public List<GLLedgerDetailsVwBO> LedgerMasterDetails { get; set; }
        public List<GLVoucherApprovedInfoBO> VoucherApproval { get; set; }

        public ArrayList CpBpCrBrAccountHead = new ArrayList();

        public decimal TotalDrAmount { get; set; }
        public decimal TotalCrAmount { get; set; }
        public long RandomLedgerMasterId { get; set; }
    }
}
