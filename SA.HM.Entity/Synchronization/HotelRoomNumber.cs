namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class HotelRoomNumber
    {
        [Key]
        public int RoomId { get; set; }

        public int? RoomTypeId { get; set; }

        [StringLength(50)]
        public string RoomNumber { get; set; }

        [StringLength(300)]
        public string RoomName { get; set; }

        public bool? IsSmokingRoom { get; set; }

        public int? StatusId { get; set; }

        public long? HKRoomStatusId { get; set; }

        [StringLength(50)]
        public string CleanupStatus { get; set; }

        public DateTime? CleanDate { get; set; }

        public DateTime? LastCleanDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
