using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentDetailsViewBO 
    {
        public Nullable<long> CompanyBillId { get; set; }
        public long PaymentDetailsId { get; set; }
        public long PaymentId { get; set; }
        public Nullable<long> CompanyBillDetailsId { get; set; }
        public long CompanyPaymentId { get; set; }
        public long BillId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string BillNumber { get; set; }
        public string ModuleName { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
    }
}
