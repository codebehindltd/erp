using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLOpeningBalanceDetail
    {
        public long Id { get; set; }
        public long GLOpeningBalanceId { get; set; }
        public long AccountNodeId { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public decimal OpeningBalance { get; set; }
    }
}
