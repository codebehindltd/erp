using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class ServiceChargeConfigurationBO
    {
        public long ServiceChargeConfigurationId { get; set; }
        public decimal? ServiceAmount { get; set; }
        public Int16 TotalEmployee { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmpTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
