using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class SupplierPaymentBO
    {

        public Int64 PaymentId { get; set; }
        public string PaymentFor { get; set; }
        public string AdjustmentType { get; set; }
        public Int64 SupplierPaymentAdvanceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateDisplay { get; set; }
        public DateTime? ChecqueDate { get; set; }
        public string ChecqueDateDisplay { get; set; }
        public string LedgerNumber { get; set; }
        public string CurrencyType { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public int SupplierId { get; set; }
        public string Remarks { get; set; }
        public string PaymentType { get; set; }
        public int AccountingPostingHeadId { get; set; }
        public string AccountingPostingHead { get; set; }
        public string SupplierName { get; set; }
        public string ChequeNumber { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ConvertionRate { get; set; }
        public string ApprovedStatus { get; set; }
        public int? AdjustmentAccountHeadId { get; set; }
        public decimal? PaymentAdjustmentAmount { get; set; }
    }
}
