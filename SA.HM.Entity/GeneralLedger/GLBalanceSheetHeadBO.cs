using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLBalanceSheetHeadBO
    {
        public long RCId { get; set; }
        public long AncestorId { get; set; }
        public bool IsAccountHead { get; set; }
        public int NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public int Lvl { get; set; }
        public string GroupName { get; set; }
        public string ReportType { get; set; }
        public string AccountType { get; set; }
        public string CalculationType { get; set; }
        public bool IsActiveLinkUrl { get; set; }

        public int SetupId { get; set; }
    }
}
