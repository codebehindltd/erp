using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class AccountManagerSalesTargetVwBO
    {
        public AccountManagerSalesTargetBO salesTarget = new AccountManagerSalesTargetBO();
        public List<AccountManagerSalesTargetDetailsBO> salesTargetDetails = new List<AccountManagerSalesTargetDetailsBO>();
        public string SalesTargetTable { get; set; }
    }
}
