using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestRegistrationMappingBO
    {
        public long GuestRegistrationId { get; set; }
        public long RegistrationId { get; set; }
        public long GuestId { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? PaxInRate { get; set; }
    }
}
