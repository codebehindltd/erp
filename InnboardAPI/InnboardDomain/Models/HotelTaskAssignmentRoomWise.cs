namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelTaskAssignmentRoomWise")]
    public partial class HotelTaskAssignmentRoomWise
    {
        [Key]
        public long RoomTaskId { get; set; }

        public long TaskId { get; set; }

        public int RoomId { get; set; }

        [StringLength(500)]
        public string TaskDetails { get; set; }

        [StringLength(20)]
        public string TaskStatus { get; set; }

        public long? OldHKRoomStatusId { get; set; }

        [StringLength(100)]
        public string OldHKRoomStatus { get; set; }

        public long? HKRoomStatusId { get; set; }

        public DateTime? FeedbackTime { get; set; }

        [StringLength(500)]
        public string Feedbacks { get; set; }

        public DateTime? InTime { get; set; }

        public DateTime? OutTime { get; set; }

        public int? EmpId { get; set; }
    }
}
