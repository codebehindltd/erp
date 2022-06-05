namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomLogFile")]
    public partial class HotelRoomLogFile
    {
        [Key]
        public long RoomLogFileId { get; set; }

        public int? RoomId { get; set; }

        public int? StatusId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
