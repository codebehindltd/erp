using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class Alert
    {
        public string AlertType { get; set; }
        public string Message { get; set; }
        public byte IsSuccess { get; set; }

        public int TimeToDisplay { get; set; }
        public string RederictUrl { get; set; }
    }
}