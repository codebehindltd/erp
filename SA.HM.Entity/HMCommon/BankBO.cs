using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class BankBO
    {
        public int BankId { get; set; }
        public int BankHeadId { get; set; }
        public string BankName { get; set; }        
        public string AccountName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string BankAccountNameAndNumber { get; set; }
        public string AccountType { get; set; }
        public string Description { get; set; }        
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
