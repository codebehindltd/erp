using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePaymentBO
    {
        public Int64 PaymentId { get; set; }
        public Int64 EmployeeBillId { get; set; }
        public string PaymentFor { get; set; }
        public string AdjustmentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string LedgerNumber { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public int EmployeeId { get; set; }
        public string Remarks { get; set; }
        public string PaymentType { get; set; }
        public int AccountingPostingHeadId { get; set; }
        public string EmployeeName { get; set; }
        public string ChequeNumber { get; set; }
        public int? CurrencyId { get; set; }
        public Int64 EmployeePaymentAdvanceId { get; set; }
        public decimal? ConvertionRate { get; set; }
        public string ApprovedStatus { get; set; }
        public int? AdjustmentAccountHeadId { get; set; }
        public decimal? PaymentAdjustmentAmount { get; set; }
    }
}
