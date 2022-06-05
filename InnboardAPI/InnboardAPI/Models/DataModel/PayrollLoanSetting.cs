namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollLoanSetting")]
    public partial class PayrollLoanSetting
    {
        [Key]
        public int LoanSettingId { get; set; }

        public decimal CompanyLoanInterestRate { get; set; }

        public decimal PFLoanInterestRate { get; set; }

        public int MaxAmountCanWithdrawFromPFInPercentage { get; set; }

        public decimal MinPFMustAvailableToAllowLoan { get; set; }

        public decimal MinJobLengthToAllowCompanyLoan { get; set; }

        public int DurationToAllowNextLoanAfterCompletetionTakenLoan { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
