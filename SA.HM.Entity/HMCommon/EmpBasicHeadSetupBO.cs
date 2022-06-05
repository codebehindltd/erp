using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class EmpBasicHeadSetupBO
    {
        public int BasicSetupId { get; set; }
        public int SalaryHeadId { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
