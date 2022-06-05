namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollEmployeeBillGenerationDetails
    {
        [Key]
        public long EmployeeBillDetailsId { get; set; }

        public long EmployeeBillId { get; set; }

        public long EmployeePaymentId { get; set; }

        public int BillId { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? DueAmount { get; set; }
    }
}
