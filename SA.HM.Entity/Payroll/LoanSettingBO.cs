using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LoanSettingBO
    {
        public int LoanSettingId { get; set; }
        public decimal CompanyLoanInterestRate { get; set; }
        public decimal PFLoanInterestRate { get; set; }
        public decimal MaxAmountCanWithdrawFromPFInPercentage { get; set; }
        public decimal MinPFMustAvailableToAllowLoan { get; set; }
        public decimal MinJobLengthToAllowCompanyLoan { get; set; }
        public int DurationToAllowNextLoanAfterCompletetionTakenLoan { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
