using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationBillPaymentBO
    {
        public long Id { get; set; }
        public long ReservationId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReportPaymentDate { get; set; }
        public long FieldId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string CurrencyHead { get; set; }
        public string PaymentMode { get; set; }
        public long BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime ChecqueDate { get; set; }
        public int ChecqueCompanyId { get; set; }
        public string CardNumber { get; set; }
        public string CardReference { get; set; }
        public int IsPaymentTransfered { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public long DealId { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CardType { get; set; }
        public string PaymentType { get; set; }

        public string PaymentDateStringFormat { get; set; }

        public string ReservedCompany { get; set; }
        public string CurrencyType { get; set; }
    }
}
