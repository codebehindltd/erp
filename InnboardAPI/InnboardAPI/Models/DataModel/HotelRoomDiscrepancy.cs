namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRoomDiscrepancy")]
    public partial class HotelRoomDiscrepancy
    {
        [Key]
        public long RoomDiscrepancyId { get; set; }

        public int RoomId { get; set; }

        public long? TaskId { get; set; }

        public long HKRoomStatusId { get; set; }

        public int? FOPersons { get; set; }

        public int? HKPersons { get; set; }

        [StringLength(500)]
        public string DiscrepanciesDetails { get; set; }

        public DateTime? AssignDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
