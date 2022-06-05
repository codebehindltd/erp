namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestRoomShiftInfo")]
    public partial class HotelGuestRoomShiftInfo
    {
        [Key]
        public int RoomShiftId { get; set; }

        public int? RegistrationId { get; set; }

        public int? PreviousRoomId { get; set; }

        public int? ShiftedRoomId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
