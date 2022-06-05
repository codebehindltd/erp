using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class DailySalesStatementViewBO
    {
        public List<DailySalesStatementBO> SalesStatementSingleDate { get; set; }
        public List<DailySalesStatementBO> SalesStatementSummary { get; set; }
    }
}
