using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class OpeningBalanceAccountList
    {
        public Int64 AssetNodeId { get; set; }
        public string AssetNodeHead { get; set; }
        public string AssetNodeType { get; set; }
        public decimal AssetAmount { get; set; }

        public Int64 LiabilitiesNodeId { get; set; }
        public string LiabilitiesNodeHead { get; set; }
        public string LiabilitiesNodeType { get; set; }
        public decimal LiabilitiesAmount { get; set; }
    }
}
