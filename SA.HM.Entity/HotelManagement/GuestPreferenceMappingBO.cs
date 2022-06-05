using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestPreferenceMappingBO
    {
        public long MappingId { get; set; }
        public int GuestId { get; set; }
        public long PreferenceId { get; set; }
    }
}
