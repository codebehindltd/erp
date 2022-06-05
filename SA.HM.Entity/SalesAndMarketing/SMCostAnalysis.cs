using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCostAnalysis
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrandTotal { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CalculatedDiscountAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
    }
}
