using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class SMSView
    {
        public string From { get; set; }

        public string FromDisplayName { get; set; }

        public string Body { get; set; }
        public string TempleteName { get; set; }
    }
}
