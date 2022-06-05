namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        
        public int? TotalRooms { get; set; }
        
    }
}
