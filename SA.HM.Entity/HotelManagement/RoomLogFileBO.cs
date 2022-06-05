using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomLogFileBO
    {
        public int RoomLogFileId { get; set; }
        public int RoomId { get; set; }
        public int StatusId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }
        public int RoomTypeId { get; set; }
        public string StatusName { get; set; }        
        
    }
}
