using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelLinkedRoomDetailsBO
    {
        public long Id { get; set; }
        public long MasterId { get; set; }
        public long RegistrationId { get; set; }
        public long RoomId { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string RoomNumber { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
