using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonNodeMatrixBO
    {
        public Int32 NodeId { get; set; }
        public Int32 AncestorId { get; set; }
        public string AncestorHead { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string HeadWithCode { get; set; }        
        public int Lvl { get; set; }

        public int CFSetupId { get; set; }
        public int CFHeadId { get; set; }
        public int PLSetupId { get; set; }
        public int PLHeadId { get; set; }

        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public Boolean NodeMode { get; set; }
        public string ActiveStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
