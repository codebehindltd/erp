using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpGradeBO
    {
        public int GradeId { get; set; }
        public string Name { get; set; }
        public int ProvisionPeriodId { get; set; }
        public int ProvisionPeriodMonth { get; set; }
        public bool IsManagement { get; set; }
        public string IsManagementText { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatName { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public decimal? BasicAmount { get; set; }

    }
}
