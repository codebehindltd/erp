namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpExperience")]
    public partial class PayrollEmpExperience
    {
        [Key]
        public int ExperienceId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        [StringLength(200)]
        public string CompanyUrl { get; set; }

        public DateTime JoinDate { get; set; }

        [StringLength(200)]
        public string JoinDesignation { get; set; }

        public DateTime? LeaveDate { get; set; }

        [StringLength(200)]
        public string LeaveDesignation { get; set; }

        [StringLength(500)]
        public string Achievements { get; set; }
    }
}
