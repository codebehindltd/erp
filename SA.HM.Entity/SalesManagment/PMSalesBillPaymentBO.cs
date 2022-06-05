using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class PMSalesBillPaymentBO
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public decimal PaymentLocalAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public int FieldId { get; set; }
        public int NodeId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }

        public string CardType { get; set; }
        public string PaymentType { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal PaymentAmout { get; set; }
        public string PaymentMode { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime ChecqueDate { get; set; }
        public string CardNumber { get; set; }
        public string CardReference { get; set; }
        public DateTime? ExpireDate { get; set; }
        public String CardHolderName { get; set; }
        public int PayMode { get; set; }
    }
}
