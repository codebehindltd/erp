using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.FixedAsset
{
    public class FADepreciationDetailsBO
    {
        public long Id { get; set; }
        public long DepreciationId { get; set; }
        public long TransactionNodeId { get; set; }
        public decimal? DepreciationPercentage { get; set; }
    }
}
