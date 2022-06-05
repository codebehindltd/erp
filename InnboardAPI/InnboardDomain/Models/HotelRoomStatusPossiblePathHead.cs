namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomStatusPossiblePathHead")]
    public partial class HotelRoomStatusPossiblePathHead
    {
        [Key]
        public int PathId { get; set; }

        [StringLength(300)]
        public string PossiblePath { get; set; }

        [StringLength(200)]
        public string DisplayText { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
