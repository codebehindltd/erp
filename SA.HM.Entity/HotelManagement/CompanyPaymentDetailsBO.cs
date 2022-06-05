using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentDetailsBO
    {
        public Int64 PaymentDetailsId { get; set; }
        public Int64 PaymentId { get; set; }
        public Int64 CompanyBillDetailsId { get; set; }
        public Int64 CompanyPaymentId { get; set; }
        public int BillId { get; set; }
        public decimal PaymentAmount { get; set; }
        public Int64 PaymentHeadId { get; set; }
        public Int64 CurrencyTypeId { get; set; }
        public Int64 Id { get; set; }
        public Int64 DetailId { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentHeadName { get; set; }
        public string CurrencyTypeName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal Totalamount { get; set; }
        public decimal ConversionRate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ChequeDate { get; set; }






    }
}
