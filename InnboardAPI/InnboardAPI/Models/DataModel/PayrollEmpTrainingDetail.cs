namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTrainingDetail")]
    public partial class PayrollEmpTrainingDetail
    {
        [Key]
        public int TrainingDetailId { get; set; }

        public int? TrainingId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
