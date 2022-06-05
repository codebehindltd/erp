using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelTaskAssignmentWiseRoomBO
    {
        public long RoomAssignListId { get; set; }
        public long TaskId { get; set; }
        public int RoomId { get; set; }        
    }
}
