using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class InvOpeningBalanceDetail
    {
        public long Id { get; set; }
        public long InvOpeningBalanceId { get; set; }
        public string TransactionNodeName { get; set; }
        public long TransactionNodeId { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? Total { get; set; }
    }
}
