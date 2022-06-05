using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLVoucherApprovalVwBO
    {
        public long LedgerMasterId { get; set; }
        public string GLStatus { get; set; }
        public int ApprovedRCheckedby { get; set; }
        public int CreatedBy { get; set; }
    }
}
