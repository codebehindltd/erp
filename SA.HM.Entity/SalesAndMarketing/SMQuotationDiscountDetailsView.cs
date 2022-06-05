using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationDiscountDetailsView
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountAmountUSD { get; set; }
    }
}
