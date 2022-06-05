namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpAdvanceTaken")]
    public partial class PayrollEmpAdvanceTaken
    {
        [Key]
        public int AdvanceId { get; set; }

        public int EmpId { get; set; }

        public DateTime AdvanceDate { get; set; }

        public decimal AdvanceAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string PayMonth { get; set; }

        public bool IsDeductFromSalary { get; set; }

        public int? ApprovedBy { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? CheckedBy { get; set; }

        [StringLength(15)]
        public string ApprovedStatus { get; set; }
    }
}
