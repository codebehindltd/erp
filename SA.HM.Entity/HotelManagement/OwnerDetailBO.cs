using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class OwnerDetailBO
    {
        public int DetailId { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal CommissionValue { get; set; }
    }
}
