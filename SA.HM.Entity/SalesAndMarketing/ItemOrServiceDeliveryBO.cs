using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class ItemOrServiceDeliveryBO
    {
        public int ItemServiceDeliveryId { get; set; }
        public string DeliveryTypeName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
