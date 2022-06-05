using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesCallEntryView: SalesCallEntryBO
    {
        public List<SalesCallParticipantBO> participants { get; set; }
        public List<SalesCallParticipantBO> OfficeParticipants { get; set; }
    }
}
