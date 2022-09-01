using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLCompanyBO
    {
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int InterCompanyTransactionHeadId { get; set; }
        public string CompanyType { get; set; }
        public string ImageName { get; set; }
        public bool IsProfitableOrganization { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string CompanyAddress { get; set; }
        public string WebAddress { get; set; }
        public string Telephone { get; set; }
        public string HotLineNumber { get; set; }
        public string BinNumber { get; set; }
        public string TinNumber { get; set; }
        public string BudgetType { get; set; }
    }
}
