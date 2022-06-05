using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLCashFlowHeadBO
    {
        public int HeadId { get; set; }
        public string CashFlowHead { get; set; }
        public int GroupId { get; set; }
        public string GroupHead { get; set; }

        public string NotesNumber { get; set; }
        public string CashFlowHeadWithNotes { get; set; }

        public string CashFlowHeadWithGroup { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public int CFSetupId { get; set; }
    }
}
