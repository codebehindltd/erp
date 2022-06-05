using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
   public class GLProjectBO
    {

        public int ProjectId { get; set; }
        public int CompanyId { get; set; }
        public string GLCompany { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CodeAndName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StageId { get; set; }
        public string ProjectStage { get; set; }
        public Boolean IsFinalStage { get; set; }
        public decimal ProjectAmount { get; set; }
        public long? ProjectCompanyId { get; set; }

        public List<GLProjectWiseCostCenterMappingBO> CostCenters { get; set; }
        public GLProjectBO()
        {
            CostCenters = new List<GLProjectWiseCostCenterMappingBO>();
        }
    }
}
