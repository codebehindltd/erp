using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class LCPaymentBO
    {
        public long PaymentId { get; set; }        
        public decimal Amount { get; set; }
        public decimal LocalCurrencyAmount { get; set; }
        public long LCPaymentId { get; set; }
        public string PaymentType { get; set; }
        public string BillNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public long LCId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal DRAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public string Remarks { get; set; }
        public string PaymentStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string LCNumber { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyType { get; set; }

        public int AccountHeadId { get; set; }
        public string AccountHeadName { get; set; }
    }
}
