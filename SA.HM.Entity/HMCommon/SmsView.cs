using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class SmsView
    {
        public string From { get; set; }
       
        public string FromDisplayName { get; set; }
       
        public string Body { get; set; }
        public string TempleteName { get; set; }
    }
}
