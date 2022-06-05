using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class NodeMatrixBO
    {
        public Int64 NodeId { get; set; }
        public Int32? AncestorId { get; set; }
        public string AncestorHead { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string HeadWithCode { get; set; }
        public int Lvl { get; set; }

        public int CFSetupId { get; set; }
        public int CFHeadId { get; set; }
        public int PLSetupId { get; set; }
        public int PLHeadId { get; set; }
        public int BSSetupId { get; set; }
        public int BSHeadId { get; set; }

        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public Boolean NodeMode { get; set; }
        public string NodeType { get; set; }
        public string ActiveStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }
        public string NodeHeadDisplay { get; set; }
        public string Nature { get; set; }
        public string CashFlowNotes { get; set; }
        public string NotesNumber { get; set; }
        public string ProfitNLossNotes { get; set; }

        public bool? IsTransactionalHead { get; set; }


        //public int NodeId { get; set; }
        //public Nullable<int> AncestorId { get; set; }
        //public string NodeNumber { get; set; }
        //public string NodeHead { get; set; }
        //public string GoogleSearch { get; set; }
        //public int Lvl { get; set; }
        //public string Hierarchy { get; set; }
        //public string HierarchyIndex { get; set; }
        //public bool NodeMode { get; set; }
        //public string NodeType { get; set; }
        //public string ActiveStatus { get; set; }
        //public Nullable<bool> IsTransactionalHead { get; set; }

    }
}
