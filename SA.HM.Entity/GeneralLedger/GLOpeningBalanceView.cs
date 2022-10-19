using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLOpeningBalanceView
    {
        public GLOpeningBalance OpeningBalance { get; set; }
        public List<GLOpeningBalance> CompanyDebitCreditList { get; set; }
        public List<GLOpeningBalance> SupplierDebitCreditList { get; set; }
        public List<GLOpeningBalance> EmployeeDebitCreditList { get; set; }
        public List<GLOpeningBalance> MemberDebitCreditList { get; set; }
        public List<GLOpeningBalance> CNFDebitCreditList { get; set; }
        public List<GLOpeningBalanceDetail> OpeningBalanceDetails { get; set; }
        public List<OpeningBalanceAccountList> AccountOpeningBalance { get; set; }
        public InvOpeningBalance InvOpeningBalance { get; set; }
        public List<InvOpeningBalanceDetail> InvOpeningBalanceDetails { get; set; }
        public string TableString { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
        public GLOpeningBalanceView()
        {
            OpeningBalanceDetails = new List<GLOpeningBalanceDetail>();
            InvOpeningBalanceDetails = new List<InvOpeningBalanceDetail>();
        }
    }
}
