using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CallToActionParticipantBO
    {
        public long Id { get; set; }
        public long CallToActionDetailsId { get; set; }
        public string PrticipantType { get; set; }
        public long ContactId { get; set; }
    }
}
