using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class TemporaryReplaceBO
    {
        public int Id { get; set; }
        public string BodyText { get; set; }
        public string ReplacedBy { get; set; }
        public string ReplaceByValue { get; set; }
    }
}
