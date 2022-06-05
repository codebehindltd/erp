namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAllowanceDeductionHead")]
    public partial class PayrollAllowanceDeductionHead
    {
        [Key]
        public int AllowDeductId { get; set; }

        [Required]
        [StringLength(200)]
        public string AllowDeductName { get; set; }

        [Required]
        [StringLength(50)]
        public string AllowDeductType { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
