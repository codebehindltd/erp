using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class QuationWiseSalesListBO
    {
        public List<SMQuotationWiseSalesBO> quotationWiseSales { get; set; }
        public List<SMQuotationDetailsBO> itemDetails { get; set; }
        public List<SMQuotationDetailsBO> seviceDetails { get; set; }
    }
}
