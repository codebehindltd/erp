using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationClassificationDiscountBO
    {
        public long Id { get; set; }
        public long ReservationId { get; set; }        
        public int CategoryId { get; set; }
    }
}
