using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class FinishedProductBO
    {
        public int FinishProductId { get; set; }
        public long Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public string ProductionDateDisplay { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDateDisplay { get; set; }
        public int CostCenterId { get; set; }
        public string ApprovedStatus { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CostCenter { get; set; }        
    }
}
