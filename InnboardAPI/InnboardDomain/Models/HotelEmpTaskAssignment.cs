namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelEmpTaskAssignment")]
    public partial class HotelEmpTaskAssignment
    {
        [Key]
        public long TaskId { get; set; }

        public int TaskSequence { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AssignDate { get; set; }

        [StringLength(10)]
        public string Shift { get; set; }

        [StringLength(50)]
        public string RoomNumber { get; set; }

        public int? FloorId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
