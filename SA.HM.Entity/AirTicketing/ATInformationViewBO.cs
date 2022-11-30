using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.AirTicketing
{
    public class ATInformationViewBO
    {
        public AirlineTicketMasterBO ATMasterInfo { get; set; }
        public List<AirlineTicketInfoBO> ATInformationDetails { get; set; }
        public List<GuestBillPaymentBO> ATPaymentInfo { get; set; }
    }
}
