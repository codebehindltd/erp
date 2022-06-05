using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestBillInfoDetailsBO
    {
        public int PaymentId { get; set; }
        public string BillNumber { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string CurrencyType { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<int> ServiceBillId { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public Nullable<System.DateTime> ChecqueDate { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string CardHolderName { get; set; }
        public string CardReference { get; set; }
        public string PaymentDescription { get; set; }
        public string TransactionType { get; set; }
    }
}
