namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomOwnerDetail")]
    public partial class HotelRoomOwnerDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? OwnerId { get; set; }

        public int? RoomId { get; set; }

        public decimal? CommissionValue { get; set; }
    }
}
