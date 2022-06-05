using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelDailyRoomConditionBO
    {
        public long DailyRoomConditionId { get; set; }
        public int RoomId { get; set; }
        public long RoomConditionId { get; set; }
        public DateTime AssignDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string Condition { get; set; }
    }
}
