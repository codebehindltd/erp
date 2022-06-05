namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelDailyRoomCondition")]
    public partial class HotelDailyRoomCondition
    {
        [Key]
        public long DailyRoomConditionId { get; set; }

        public int RoomId { get; set; }

        public long RoomConditionId { get; set; }

        public DateTime? AssignDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
