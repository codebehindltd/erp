using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemStockVarianceBO
    {
        public int StockVarianceId { get; set; }
        public DateTime StockVarianceDate { get; set; }
        public int CostCenterId { get; set; }
        public string ApprovedStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string CostCenter { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
    }
}
