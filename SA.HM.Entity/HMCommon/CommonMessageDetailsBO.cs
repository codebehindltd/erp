using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonMessageDetailsBO : CommonMessageBO
    {
        public long MessageDetailsId { get; set; }
        public long MessageId { get; set; }
        public int MessageTo { get; set; }
        public string UserId { get; set; }
        public bool IsReaden { get; set; }
        public string UserName { get; set; }
    }
}
