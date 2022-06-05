using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnboardAPI.Models
{
    public class RoomNumberBO
    {
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> RoomTypeId { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public string RoomName { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string ActiveStatus { get; set; }
    }
}