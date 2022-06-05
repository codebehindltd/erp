namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomInventory")]
    public partial class HotelRoomInventory
    {
        [Key]
        public int InventoryOutId { get; set; }

        public DateTime OutDate { get; set; }

        public int RoomTypeId { get; set; }

        public int RoomId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(15)]
        public string Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
