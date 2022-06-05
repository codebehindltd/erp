using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class BudgetStatementReportDTO
    {
        public Nullable<int> GroupId { get; set; }
        public string GroupNodeHead { get; set; }
        public Nullable<int> GroupOrder { get; set; }
        public Nullable<int> GroupLevel { get; set; }
        public Nullable<long> NodeId { get; set; }
        public string NodeHead { get; set; }
        public string NodeNumber { get; set; }
        public Nullable<int> NodeOrder { get; set; }
        public Nullable<int> BudgetParentId { get; set; }
        public string BudgetParentName { get; set; }
        public Nullable<int> BudgetGroupId { get; set; }
        public string BudgetGroupName { get; set; }
        public Nullable<int> BudgetChildGroupId { get; set; }
        public string BudgetChildGroupName { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
