using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLFiscalYearProjectMappingBO
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public int ProjectId { get; set; }
    }
}
