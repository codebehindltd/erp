using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesTransferReportViewBO
    {
        public string TransferNumber { get; set; }
        public string QuotationNo { get; set; }
        public string DealName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
    }
}
