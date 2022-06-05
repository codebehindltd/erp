using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
   public class RestaurantBillPaymentBO
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public int BillId { get; set; }
        public int RegistrationId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public int FieldId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmout { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime? ChecqueDate { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CardHolderName { get; set; }
        public string CardReference { get; set; }
        public int RefundAccountHead { get; set; }
        public int DealId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
