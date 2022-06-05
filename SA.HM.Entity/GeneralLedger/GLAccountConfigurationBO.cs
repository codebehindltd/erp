using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
   public class GLAccountConfigurationBO: NodeMatrixBO
   {
       public int ConfigurationId { get; set; }
       public string AccountType { get; set; }
       //public int NodeId { get; set; }
       //public string NodeHead { get; set; }
       public string AccountTypeName { get; set; }
       public int ProjectId { get; set; }
       public int CompanyId { get; set; }
       public string ProjectName { get; set; }
       //public int CreatedBy { get; set; }
       public string CreatedDate { get; set; }
      // public int LastModifiedBy { get; set; }
       public string LastModifiedDate { get; set; }
       public int HeadCount { get; set; }
    }
}
