using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMDealWiseContactMap
    {
        public long Id { get; set; }
        public long DealId { get; set; }
        public long ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

    }
}
