namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTimeSlab")]
    public partial class PayrollEmpTimeSlab
    {
        [Key]
        public int EmpTimeSlabId { get; set; }

        public int EmpId { get; set; }

        public DateTime? SlabEffectDate { get; set; }

        public int TimeSlabId { get; set; }

        [StringLength(20)]
        public string WeekEndMode { get; set; }

        [StringLength(20)]
        public string WeekEndFirst { get; set; }

        [StringLength(20)]
        public string WeekEndSecond { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
