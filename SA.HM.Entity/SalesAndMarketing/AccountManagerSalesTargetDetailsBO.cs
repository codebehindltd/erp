using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class AccountManagerSalesTargetDetailsBO
    {
        public Int64 TargetDetailsId { get; set; }
        public Int64 TargetId { get; set; }
        public Int16 MonthId { get; set; }
        public Int32 AccountManagerId { get; set; }
        public decimal Amount { get; set; }
        public string MonthName { get; set; }
        public string FiscalYearName { get; set; }
        public string AccountManager { get; set; }
    }
}
