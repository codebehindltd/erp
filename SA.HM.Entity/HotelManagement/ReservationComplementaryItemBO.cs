using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ReservationComplementaryItemBO
    {
        public int RCItemId { get; set; }
        public int ReservationId { get; set; }
        public int ComplementaryItemId { get; set; }
    }
}
