using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBearerBO
    {
        public int BearerId { get; set; }
        public int UserInfoId { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string UserName { get; set; }
        public string BearerPassword { get; set; }
        public bool IsBearer { get; set; }
        public bool IsChef { get; set; }
        public bool IsRestaurantBillCanSettle { get; set; }
        public bool IsItemCanEditDelete { get; set; }
        public bool IsItemSearchEnable { get; set; }
        public string RestaurantBillCanSettleStatus { get; set; }
        public string PermittedCostcenter { get; set; }
        public string IsItemSearchEnableStatus { get; set; }
        public string IsBearerStatus { get; set; }
    }
}
