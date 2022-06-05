using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class PaidServiceViewBO
    {
        public List<HotelGuestServiceInfoBO> PaidService { get; set; }
        public List<RegistrationServiceInfoBO> RegistrationPaidService { get; set; }
    }
}
