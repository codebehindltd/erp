using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemSpecialRemarksBO
    {
        public int SpecialRemarksId { get; set; }
        public string SpecialRemarks { get; set; }
        public string Description { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
