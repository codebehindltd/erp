using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class PaymentModeBO
    {
        public Int32 PaymentModeId { get; set; }
        public Int32? AncestorId { get; set; }
        public string AncestorHead { get; set; }
        public string PaymentMode { get; set; }
        public string DisplayName { get; set; }        
        public string PaymentCode { get; set; }
        public string HeadWithCode { get; set; }
        public int Lvl { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public Boolean ActiveStat { get; set; }
        public string NodeType { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
