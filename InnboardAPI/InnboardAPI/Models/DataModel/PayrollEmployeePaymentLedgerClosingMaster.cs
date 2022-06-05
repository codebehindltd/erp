namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmployeePaymentLedgerClosingMaster")]
    public partial class PayrollEmployeePaymentLedgerClosingMaster
    {
        [Key]
        public long YearClosingId { get; set; }

        public int FiscalYearId { get; set; }

        public int? EmployeeId { get; set; }

        public int? ProjectId { get; set; }

        public int? DonorId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProfitLossClosing { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
