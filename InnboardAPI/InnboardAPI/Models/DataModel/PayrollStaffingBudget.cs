namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollStaffingBudget")]
    public partial class PayrollStaffingBudget
    {
        [Key]
        public long StaffingBudgetId { get; set; }

        public int DepartmentId { get; set; }

        [Required]
        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
