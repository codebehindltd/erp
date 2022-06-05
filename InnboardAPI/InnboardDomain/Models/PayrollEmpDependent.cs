namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpDependent")]
    public partial class PayrollEmpDependent
    {
        [Key]
        public int DependentId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(200)]
        public string DependentName { get; set; }

        [StringLength(100)]
        public string Relationship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string Age { get; set; }
    }
}
