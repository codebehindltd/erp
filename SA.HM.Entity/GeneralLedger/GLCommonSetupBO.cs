using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
   public class GLCommonSetupBO
    {   
        public int SetupId { get; set; }
        public int ProjectId { get; set; }
        public string TypeName { get; set; }
        public string SetupName { get; set; }
        public string SetupValue { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
