using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CallToActionViewBO
    {
        public CallToActionBO CallToAction { get; set; }
        public List<CallToActionDetailBO> CallToActionDetailList { get; set; }
    }
}
