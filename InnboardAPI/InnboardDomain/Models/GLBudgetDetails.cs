namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLBudgetDetails
    {
        [Key]
        public long BudgetDetailsId { get; set; }

        public long BudgetId { get; set; }

        public short MonthId { get; set; }

        public long NodeId { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
    }
}
