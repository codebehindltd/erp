using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SupportAndTicket
{
    public class STSupportDetailsBO
    {
        public long STSupportDetailsId { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public int? StockBy { get; set; }
        public string Type { get; set; }
        public string HeadName { get; set; }
        public string ItemName { get; set; }
        public int? STSupportId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UnitQuantity { get; set; }
        public decimal? VatRate { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
