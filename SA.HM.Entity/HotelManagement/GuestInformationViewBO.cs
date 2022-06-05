using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestInformationViewBO
    {
        public GuestInformationBO GuestInfo { get; set; }
        public List<DocumentsBO> GuestDoc { get; set; }
        public string GuestPreference { get; set; }
        public string GuestPreferenceId { get; set; }
    }
}
