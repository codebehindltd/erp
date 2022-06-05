using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLRevenueDetailsBO
    {
        public Nullable<int> GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<short> GroupSortingOrder { get; set; }
        public Nullable<int> SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public Nullable<short> SubGroupSortingOrder { get; set; }
        public Nullable<int> ChildId { get; set; }
        public Nullable<int> NodeId { get; set; }
        public string ChildName { get; set; }
        public string NodeType { get; set; }
        public Nullable<short> ChildGroupSortingOrder { get; set; }
        public Nullable<decimal> CurrentMonthActual { get; set; }
        public Nullable<decimal> CurrentMonthBudgetAmount { get; set; }
        public Nullable<decimal> LasYearCurrentMonthActual { get; set; }
        public Nullable<decimal> YearToDateActual { get; set; }
        public Nullable<decimal> YearToDateBudget { get; set; }
        public Nullable<decimal> LastYearToDateActual { get; set; }

        public string CurrentMonthCaption { get; set; }
        public string PreviousMonthCaption { get; set; }
        public string CurrentYearToMonthCaption { get; set; }
        public string PreviousYearToMonthCaption { get; set; }

    }
}
