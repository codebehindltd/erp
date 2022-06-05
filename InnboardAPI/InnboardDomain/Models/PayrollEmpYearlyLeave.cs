namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpYearlyLeave")]
    public partial class PayrollEmpYearlyLeave
    {
        [Key]
        public int YearlyLeaveId { get; set; }

        public int? EmpId { get; set; }

        public int? LeaveTypeId { get; set; }

        public int? LeaveQuantity { get; set; }
    }
}
