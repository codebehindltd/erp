using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PrPoUserPermissionBO
    {
        public long MappingId { get; set; }
        public int EmpId { get; set; }
        public int UserInfoId { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsPRAllow { get; set; }
        public bool IsPOAllow { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string PermittedCostcenter { get; set; }

        public string UserName { get; set; }
    }
}
