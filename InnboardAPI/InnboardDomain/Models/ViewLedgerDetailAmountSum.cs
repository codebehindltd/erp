namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ViewLedgerDetailAmountSum")]
    public partial class ViewLedgerDetailAmountSum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LedgerMasterId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [StringLength(256)]
        public string InWordAmount { get; set; }
    }
}
