namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLBudget")]
    public partial class GLBudget
    {
        [Key]
        public long BudgetId { get; set; }

        public long FiscalYearId { get; set; }

        public long? CheckedBy { get; set; }

        public long? ApprovedBy { get; set; }

        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
