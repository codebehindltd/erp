namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpRoster")]
    public partial class PayrollEmpRoster
    {
        [Key]
        public int EmpRosterId { get; set; }

        public int? EmpId { get; set; }

        public int? RosterId { get; set; }

        public DateTime? RosterDate { get; set; }

        public int? TimeSlabId { get; set; }

        public int? SecondTimeSlabId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
