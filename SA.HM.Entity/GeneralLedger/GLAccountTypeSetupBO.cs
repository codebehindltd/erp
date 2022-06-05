using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLAccountTypeSetupBO
    {
        public int AccountTypeId { get; set; }
        public int NodeId { get; set; }
        public string AccountType { get; set; }
        //public int CreatedBy { get; set; }
        //public string CreatedDate { get; set; }
        //public int LastModifiedBy { get; set; }
        //public string LastModifiedDate { get; set; }
    }
}
