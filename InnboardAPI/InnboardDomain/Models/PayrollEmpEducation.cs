namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpEducation")]
    public partial class PayrollEmpEducation
    {
        [Key]
        public int EducationId { get; set; }

        public int? EmpId { get; set; }

        public int? LevelId { get; set; }

        [StringLength(200)]
        public string ExamName { get; set; }

        [StringLength(200)]
        public string InstituteName { get; set; }

        [StringLength(20)]
        public string PassYear { get; set; }

        [StringLength(200)]
        public string SubjectName { get; set; }

        [StringLength(50)]
        public string PassClass { get; set; }
    }
}
