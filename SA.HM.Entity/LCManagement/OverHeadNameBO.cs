using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class OverHeadNameBO
    {
        public int OverHeadId { get; set; }
        public string OverHeadName { get; set; }
        public string Description { get; set; }
        public int CostCenterId { get; set; }
        public int NodeId { get; set; }
        public bool ActiveStat { get; set; }
        public bool IsCNFHead { get; set; }
        public string ActiveStatus { get; set; }
        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
