namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollWorkingDay")]
    public partial class PayrollWorkingDay
    {
        [Key]
        public int WorkingDayId { get; set; }

        public int? TypeId { get; set; }

        [StringLength(50)]
        public string WorkingPlan { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string DayOffOne { get; set; }

        [StringLength(50)]
        public string DayOffTwo { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
