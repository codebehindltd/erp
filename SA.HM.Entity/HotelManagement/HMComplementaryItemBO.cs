using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HMComplementaryItemBO
    {
        public int ComplementaryItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int ReservationId { get; set; }
        public int RegistrationId { get; set; }
        public int RCItemId { get; set; }
        public bool IsDefaultItem { get; set; }
        public string DefaultItem { get; set; }
    }
}
