using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelRoomDiscrepancyBO
    {
        public long RoomDiscrepancyId { get; set; }
        public int RoomId { get; set; }
        public long TaskId { get; set; }
        public long HKRoomStatusId { get; set; }
        public int FOPersons { get; set; }
        public int HKPersons { get; set; }
        public string DiscrepanciesDetails { get; set; }
        public int CreatedBy { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string HKStatusName { get; set; }
    }
}
