using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCurrentVendorBO
    {
        public int CurrentVendorId { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
