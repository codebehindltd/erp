using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.CostofGoodsSold
{
    public class CgsTransactionHeadBO
    {
        public int TransactionHeadId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool ActiveStat { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string ActiveStatus { get; set; }
    }
}
