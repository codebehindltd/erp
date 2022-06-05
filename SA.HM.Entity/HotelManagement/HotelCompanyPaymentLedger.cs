using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelCompanyPaymentLedger
    {
        public long CompanyPaymentId { get; set; }

        [StringLength(50)]
        public string ModuleName { get; set; }

        [StringLength(15)]
        public string PaymentType { get; set; }

        public long? PaymentId { get; set; }

        [StringLength(25)]
        public string LedgerNumber { get; set; }

        public int? BillId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }
        
        public DateTime PaymentDate { get; set; }

        public int CompanyId { get; set; }

        public int CurrencyId { get; set; }
        
        public decimal? ConvertionRate { get; set; }
        
        public decimal DRAmount { get; set; }
        
        public decimal CRAmount { get; set; }
        
        public decimal? CurrencyAmount { get; set; }
        
        public decimal? PaidAmount { get; set; }
        
        public decimal? PaidAmountCurrent { get; set; }
        
        public decimal? DueAmount { get; set; }
        
        public decimal? AdvanceAmount { get; set; }
        
        public decimal? AdvanceAmountRemaining { get; set; }
        
        public decimal? DayConvertionRate { get; set; }

        public long? AccountsPostingHeadId { get; set; }
        
        public decimal? GainOrLossAmount { get; set; }
        
        public decimal? RoundedAmount { get; set; }

        [StringLength(50)]
        public string ChequeNumber { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public long? BillGenerationId { get; set; }

        public long? RefCompanyPaymentId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
