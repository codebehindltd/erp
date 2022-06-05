using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLOpeningBalanceTransactionNodeAutoComplete
    {
        public long TransactionNodeId { get; set; }
        public string NodeName { get; set; }
        public string Code { get; set; }
        public int Lvl { get; set; }
        public string Hierarchy { get; set; }
    }
}
