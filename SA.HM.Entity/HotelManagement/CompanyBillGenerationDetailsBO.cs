using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyBillGenerationDetailsBO
    {
        public long CompanyBillDetailsId { get; set; }
        public long CompanyBillId { get; set; }
        public long CompanyPaymentId { get; set; }
        public int BillId { get; set; }
        public decimal Amount { get; set; }
        public string ModuleName { get; set; }        
    }
}
