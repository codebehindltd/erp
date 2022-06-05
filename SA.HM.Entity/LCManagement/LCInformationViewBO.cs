using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class LCInformationViewBO
    {
        public LCInformationBO LCInformation { get; set; }
        public List<LCInformationDetailBO> LCInformationDetail { get; set; }
        public List<LCPaymentBO> LCPayment { get; set; }
    }
}
