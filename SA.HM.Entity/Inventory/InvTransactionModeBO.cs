using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvTransactionModeBO
    {
        public int TModeId { get; set; }
        public string HeadName { get; set; }
        public string CalculationType { get; set; }
        public bool ActiveStat { get; set; }
    }
}
