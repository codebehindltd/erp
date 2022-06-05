using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLConfigurableBalanceSheetBO
    {
        public Int32 RCId { get; set; }
        public Int32 AncestorId { get; set; }
        public Boolean IsAccountHead { get; set; }
        public Int32 NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public int Lvl { get; set; }
        public string GroupName { get; set; }
        public string ReportType { get; set; }
        public string AccountType { get; set; }
        public decimal CalculatedNodeAmount { get; set; }
        public string CalculationType { get; set; }
        public Boolean IsActiveLinkUrl { get; set; }
        public string Url { get; set; }
    }
}
