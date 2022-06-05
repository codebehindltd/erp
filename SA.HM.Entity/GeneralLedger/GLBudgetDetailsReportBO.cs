using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLBudgetDetailsReportBO
    {
        public long NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string NodeType { get; set; }
        public string GroupNodeNumber { get; set; }
        public string GroupNodeHead { get; set; }
        public int NodeLevel { get; set; }
        public int NodeOrder { get; set; }
        public int GroupOrder { get; set; }
        public int GroupLevel { get; set; }
        public Nullable<long> GroupId { get; set; }
        public long BudgetDetailsId { get; set; }
        public long BudgetId { get; set; }
        public short MonthId { get; set; }
        public decimal Amount { get; set; }

        public string MonthName { get; set; }
        public string FiscalYearName { get; set; }
    }
}
