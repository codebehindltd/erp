using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesNQuotationViewBO
    {
        public string Company { get; set; }
        public string QuotationNo { get; set; }
        public string TransferNumber { get; set; }
        public string DealName { get; set; }
        public List<SMQuotationDetailsBO> SMQuotationDetailsBOList { get; set; }
    }
}
