namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpCareerTraining")]
    public partial class PayrollEmpCareerTraining
    {
        [Key]
        public int CareerTrainingId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(200)]
        public string TrainingTitle { get; set; }

        [StringLength(500)]
        public string Topic { get; set; }

        [StringLength(200)]
        public string Institute { get; set; }

        public int? Country { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(50)]
        public string TrainingYear { get; set; }

        public int? Duration { get; set; }

        [StringLength(20)]
        public string DurationType { get; set; }
    }
}
