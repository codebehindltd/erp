namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CgsMonthlyTransaction")]
    public partial class CgsMonthlyTransaction
    {
        [Key]
        public int MonthlyTransactionId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(100)]
        public string EmpType { get; set; }

        public DateTime? InputDate { get; set; }

        public decimal? Amount { get; set; }

        public int? CreatedBy { get; set; }

        [StringLength(20)]
        public string CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [StringLength(20)]
        public string LastModifiedDate { get; set; }
    }
}
