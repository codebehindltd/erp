using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SearchDetailsInfoBO
    {
        public GuestInformationBO GuestInfo { get; set; }
        public string GuestDocuments { get; set; }
        public string GuestRegistrationHistory { get; set; }
    }
}
