using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RegistrationServiceInfoBO
    {
        public int DetailServiceId { get; set; }
        public int RegistrationId { get; set; }
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal? ServiceRate { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool IsAchieved { get; set; }

        public int CurrencyType { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
