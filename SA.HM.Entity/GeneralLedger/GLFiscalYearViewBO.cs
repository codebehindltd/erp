using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLFiscalYearViewBO: GLFiscalYearBO
    {
        public int CompanyId { get; set; }
        public List<GLFiscalYearProjectMappingBO> GLFiscalYearProjects { get; set; }
    }
}
