using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestBillPaymentInvoiceReportViewBO
    {
        public int PaymentId { get; set; }
        public string BillNumber { get; set; }
        public string PaymentType { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ReservationNumber { get; set; }
        public string GuestName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateDisplay { get; set; }
        public string PaymentMode { get; set; }
        public int FieldId { get; set; }
        public string CurrencyHead { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime ChecqueDate { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string CardHolderName { get; set; }
        public string CardReference { get; set; }
        public int RefundAccountHead { get; set; }
        public string Remarks { get; set; }        
        public string DetailDescription { get; set; }
        public string RoomNumber { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Narration { get; set; }
    }
}
