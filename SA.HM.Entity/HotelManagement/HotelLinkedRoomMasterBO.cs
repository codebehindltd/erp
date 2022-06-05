using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelLinkedRoomMasterBO
    {
        public long Id { get; set; }
        public long RegistrationId { get; set; } 
        public string LinkName { get; set; }
        public string Description { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
