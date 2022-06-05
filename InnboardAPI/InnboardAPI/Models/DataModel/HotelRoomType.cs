namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomType")]
    public partial class HotelRoomType
    {
        [Key]
        public int RoomTypeId { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        [StringLength(100)]
        public string RoomType { get; set; }

        [StringLength(10)]
        public string TypeCode { get; set; }

        public decimal? RoomRate { get; set; }

        public decimal? RoomRateUSD { get; set; }

        public bool? ActiveStat { get; set; }

        public bool? SuiteType { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? PaxQuantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalRooms { get; set; }

        public virtual GLNodeMatrix GLNodeMatrix { get; set; }
    }
}
