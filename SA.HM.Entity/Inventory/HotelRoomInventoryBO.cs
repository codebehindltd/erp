using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class HotelRoomInventoryBO
    {
        public int InventoryOutId { get; set; }
        public DateTime OutDate { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public string Remarks { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
