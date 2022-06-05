using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLBudgetDetailsBO
    {
        public Int64 BudgetDetailsId { get; set; }
        public Int64 BudgetId { get; set; }
        public Int16 MonthId { get; set; }
        public Int64 NodeId { get; set; }
        public decimal Amount { get; set; }

        public string MonthName { get; set; }
        public string FiscalYearName { get; set; }
        public string NodeHead { get; set; }
    }
}
