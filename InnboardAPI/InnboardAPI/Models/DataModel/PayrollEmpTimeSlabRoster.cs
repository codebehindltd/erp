namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTimeSlabRoster")]
    public partial class PayrollEmpTimeSlabRoster
    {
        [Key]
        public int RosterId { get; set; }

        public int? EmpTimeSlabId { get; set; }

        [StringLength(20)]
        public string DayName { get; set; }

        public int? TimeSlabId { get; set; }
    }
}
