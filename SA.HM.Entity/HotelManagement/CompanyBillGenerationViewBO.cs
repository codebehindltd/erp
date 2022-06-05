using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyBillGenerationViewBO
    {
        public CompanyBillGenerationBO BillGeneration { get; set; }
        public List<CompanyBillGenerationDetailsBO> BillGenerationDetails { get; set; }
        public List<CompanyPaymentLedgerVwBo> CompanyBill = new List<CompanyPaymentLedgerVwBo>();
    }
}
