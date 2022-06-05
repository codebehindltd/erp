namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelFloorManagement")]
    public partial class HotelFloorManagement
    {
        [Key]
        public int FloorManagementId { get; set; }

        public int FloorId { get; set; }

        public int BlockId { get; set; }

        public int RoomId { get; set; }

        public double? XCoordinate { get; set; }

        public double? YCoordinate { get; set; }

        public int? RoomWidth { get; set; }

        public int? RoomHeight { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
