namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollStaffingBudgetDetails
    {
        [Key]
        public long StaffingBudgetDetailsId { get; set; }

        public long StaffingBudgetId { get; set; }

        public int JobType { get; set; }

        [Required]
        [StringLength(25)]
        public string JobLevel { get; set; }

        public short NoOfStaff { get; set; }

        public decimal BudgetAmount { get; set; }
    }
}
