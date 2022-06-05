using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ReceiptNPaymentStatementBO
    {
        public Nullable<int> Rnk { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> NodeGroupId { get; set; }
        public string NodeGroupName { get; set; }
        public Nullable<int> SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public Nullable<int> NodeId { get; set; }
        public string NodeName { get; set; }
        public string NotesNumber { get; set; }
        public Nullable<decimal> TransactionalBalance { get; set; }
        public Nullable<decimal> TransactionalOpening { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> GroupAmount { get; set; }
        public Nullable<decimal> GrandAmount { get; set; }

        public Nullable<decimal> NetIncreasesDecreasedInCashNCashEquvalent { get; set; }
        public Nullable<decimal> CashNCashEquvalentAtEndOfPeriod { get; set; }

        //----------- For Comparision Report
        public Nullable<decimal> OpeningBalanceCurrentYear { get; set; }
        public Nullable<decimal> AmountCurrentYear { get; set; }
        public Nullable<decimal> GroupAmountCurrentYear { get; set; }
        public Nullable<decimal> GrandAmountCurrentYear { get; set; }
        public Nullable<decimal> NetIncreasesDecreasedInCashNCashEquvalentCurrentYear { get; set; }
        public Nullable<decimal> CashNCashEquvalentAtEndOfPeriodCurrentYear { get; set; }

        public Nullable<decimal> OpeningBalancePreviousYear { get; set; }
        public Nullable<decimal> AmountPreviousYear { get; set; }
        public Nullable<decimal> GroupAmountPreviousYear { get; set; }
        public Nullable<decimal> GrandAmountPreviousYear { get; set; }
        public Nullable<decimal> NetIncreasesDecreasedInCashNCashEquvalentPreviousYear { get; set; }
        public Nullable<decimal> CashNCashEquvalentAtEndOfPeriodPreviousYear { get; set; }

        public DateTime? CurrentYearDateFrom { get; set; }
        public DateTime? CurrentYearDateTo { get; set; }
        public DateTime? PreviousYearDateFrom { get; set; }
        public DateTime? PreviousYearDateTo { get; set; }
    }
}
