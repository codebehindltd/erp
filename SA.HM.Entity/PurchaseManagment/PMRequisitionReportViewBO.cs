using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMRequisitionReportViewBO
    {
        public int RequisitionId { get; set; }
        public string PRNumber { get; set; }
        public string PRDate { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string Stock { get; set; }
        public string ApprovedStatus { get; set; }
        public decimal AveragePrice { get; set; }
        public string FromCostCenter { get; set; }
        public string ToCostCenter { get; set; }
        public string ProductCategory { get; set; }
        public string Remarks { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }

    }
}
