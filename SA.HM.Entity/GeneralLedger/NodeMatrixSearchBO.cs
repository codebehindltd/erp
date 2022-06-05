using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class NodeMatrixSearchBO
    {
        public Int32 NodeId { get; set; }       
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string HeadWithCode { get; set; }       
        public string NodeType { get; set; }
    }
}
