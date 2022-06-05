namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTrainingType")]
    public partial class PayrollEmpTrainingType
    {
        [Key]
        public int TrainingTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string TrainingName { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
