using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelRoomConditionBO
    {
        public long RoomConditionId { get; set; }
        public string ConditionName { get; set; }
        public Boolean ActiveStat { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
