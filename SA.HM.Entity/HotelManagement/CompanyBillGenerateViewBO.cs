using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyBillGenerateViewBO
    {
        public Int64 CompanyBillId { get; set; }
        public Int64 CompanyBillDetailsId { get; set; }
        public Int32 CompanyId { get; set; }
        public DateTime BillDate { get; set; }
        public string CompanyBillNumber { get; set; }
        public Int64 CompanyPaymentId { get; set; }
        public Int32 BillId { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public string ModuleName { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal DueAmount { get; set; }

        public Int64 PaymentDetailsId { get; set; }

        public int BillCurrencyId { get; set; }
    }
}
