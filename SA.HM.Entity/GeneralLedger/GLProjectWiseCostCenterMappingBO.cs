using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLProjectWiseCostCenterMappingBO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string CostCenter { get; set; }
        public int CostCenterId { get; set; }
    }
}
