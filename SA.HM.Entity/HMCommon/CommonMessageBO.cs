
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonMessageBO
    {
        public long MessageId { get; set; }
        public int MessageFrom { get; set; }
        public DateTime MessageDate { get; set; }
        public string Importance { get; set; }
        public string Subjects { get; set; }
        public string MessageBody { get; set; }

        public string MessageFromUserId { get; set; }
    }
}
