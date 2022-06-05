using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class ServiceTypeBO
    {
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
