using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
   public class CompanyBankBO
    {
       
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
