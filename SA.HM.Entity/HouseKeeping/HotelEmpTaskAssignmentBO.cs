using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HotelEmpTaskAssignmentBO
    {
        public Int64 TaskId { get; set; }
        public int TaskSequence { get; set; }
        public DateTime AssignDate { get; set; }
        public string Shift { get; set; }
        public string RoomNumber { get; set; }
        public int? FloorId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
