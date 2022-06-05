using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
   public class RegistrationComplementaryItemBO
    {
        public int RCItemId { get; set; }
        public int RegistrationId { get; set; }
        public int ComplementaryItemId { get; set; }
    }
}
