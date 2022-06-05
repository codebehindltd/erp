namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmployeePayment")]
    public partial class PayrollEmployeePayment
    {
        [Key]
        public long PaymentId { get; set; }

        public long? EmployeeBillId { get; set; }

        [StringLength(50)]
        public string PaymentFor { get; set; }

        [StringLength(50)]
        public string LedgerNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        public int EmployeeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdvanceAmount { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public int? AccountingPostingHeadId { get; set; }

        [StringLength(50)]
        public string ChequeNumber { get; set; }

        public int? CurrencyId { get; set; }

        public decimal? ConvertionRate { get; set; }

        [StringLength(50)]
        public string AdjustmentType { get; set; }

        public long? EmployeePaymentAdvanceId { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdjustmentAmount { get; set; }

        [StringLength(50)]
        public string ApprovedStatus { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAdjustmentAmount { get; set; }

        public int? AdjustmentAccountHeadId { get; set; }
    }
}
