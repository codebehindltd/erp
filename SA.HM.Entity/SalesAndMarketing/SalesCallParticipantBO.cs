using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesCallParticipantBO
    {
        public long Id { get; set; }
        public long SalesCallEntryId { get; set; }
        public string PrticipantType { get; set; }
        public long ContactId { get; set; }
        public string Contact { get; set; }
    }
}
