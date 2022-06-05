using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class Email
    {
        public string From { get; set; }
        public string Password { get; set; } 
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string ToDisplayName { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentSavedPath { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string TempleteName { get; set; }
    }
}
