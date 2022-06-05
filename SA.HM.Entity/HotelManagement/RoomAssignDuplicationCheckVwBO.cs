using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity
{
    public class RoomAssignDuplicationCheckVwBO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomTypeCode { get; set; }
        public int detailRowId { get; set; }

        public int PaxQuantity { get; set; }
        public int RoomQuantity { get; set; }
        public int RoomTypeId { get; set; }
    }
}
