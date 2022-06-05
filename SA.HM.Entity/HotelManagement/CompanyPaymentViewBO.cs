using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentViewBO
    {
        public CompanyPaymentBO CompanyPayment { get; set; }
        public List<CompanyPaymentDetailsViewBO> CompanyPaymentDetails { get; set; }
        public List<CompanyPaymentDetailsBO> CompanyPaymentTransactionDetails { get; set; }
        public GuestCompanyBO Company = new GuestCompanyBO();

        public List<CompanyPaymentLedgerVwBo> CompanyGeneratedBill = new List<CompanyPaymentLedgerVwBo>();
        public List<CompanyPaymentLedgerVwBo> CompanyBill = new List<CompanyPaymentLedgerVwBo>();
    }
}
