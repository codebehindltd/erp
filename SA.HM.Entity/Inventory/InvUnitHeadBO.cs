using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvUnitHeadBO
    {
        public int UnitHeadId { get; set; }
        public string HeadName { get; set; }
        public bool ActiveStat { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedByDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedByDate { get; set; }
        public string ActiveStatus { get; set; }
    }
}
