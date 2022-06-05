using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class TaskAssignmentRoomWiseBO
    {
        public long RoomTaskId { get; set; }
        public long TaskId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int? EmpId { get; set; }
        public string TaskDetails { get; set; }
        public string TaskStatus { get; set; }
        public long HKRoomStatusId { get; set; }
        public string HKStatusName { get; set; }
        public string FORoomStatus { get; set; }
        public string Feedbacks { get; set; }
        public DateTime FeedbackTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
    }
}
