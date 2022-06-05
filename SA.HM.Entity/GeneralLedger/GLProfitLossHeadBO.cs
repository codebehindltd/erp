using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
   public class GLProfitLossHeadBO
   {

       public int PLHeadId { get; set; }
       public string PLHead { get; set; }
       public string CalculationMode { get; set; }

       public string NotesNumber { get; set; }
       public string PLHeadWithNotes { get; set; }

       public bool ActiveStat { get; set; }

       public int PLSetupId { get; set; }
   }
}
