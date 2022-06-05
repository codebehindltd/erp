using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class BusinessPromotionDetailBO
    {
        public int DetailId { get; set; }
        public int BusinessPromotionId { get; set; }
        public string TransactionType { get; set; }
        public int TransactionId { get; set; }
    }
}
