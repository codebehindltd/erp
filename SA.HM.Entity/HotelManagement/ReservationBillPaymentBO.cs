using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ReservationBillPaymentBO
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReservationNumber { get; set; }
        public string ReportPaymentDate { get; set; }
        public int FieldId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string CurrencyHead { get; set; }
        public string PaymentMode { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime ChecqueDate { get; set; }
        public string CardNumber { get; set; }
        public string CardReference { get; set; }
        public int IsPaymentTransfered { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int LastModifiedBy { get; set; }
        public int DealId { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CardType { get; set; }
        public string PaymentType { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public int AccountsPostingHeadId { get; set; }
        public string PaymentDateStringFormat { get; set; }
        public string ReservedCompany { get; set; }
        public string CurrencyType { get; set; }
        public string Remarks { get; set; }        
    }
}
