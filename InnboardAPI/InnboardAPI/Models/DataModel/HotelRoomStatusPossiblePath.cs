namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomStatusPossiblePath")]
    public partial class HotelRoomStatusPossiblePath
    {
        [Key]
        public int MappingId { get; set; }

        public int? UserGroupId { get; set; }

        [StringLength(200)]
        public string PossiblePathType { get; set; }

        public int? PathId { get; set; }

        [StringLength(200)]
        public string DisplayText { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
