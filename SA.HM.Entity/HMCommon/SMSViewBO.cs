using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class SMS
    {
        public string sid { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string clientRefId { get; set; }
        public string number { get; set; }
        public string text { get; set; }
        public string URI { get; set; }
    }
}
