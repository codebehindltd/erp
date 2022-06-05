namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCOverHeadExpense")]
    public partial class LCOverHeadExpense
    {
        [Key]
        public int ExpenseId { get; set; }

        public int? LCId { get; set; }

        public int? OverHeadId { get; set; }

        public DateTime? ExpenseDate { get; set; }

        public decimal? ExpenseAmount { get; set; }

        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
