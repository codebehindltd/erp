using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesTransferDetailsInvoiceViewBO
    {
        public List<SMSalesTransferReportViewBO> TransferNCompanyInfo { get; set; }
        public List<SMSalesTransferDetailsBO> Items { get; set; }
        public List<SMSalesItemSerialTransferBO> Serials { get; set; }
        public SMSalesTransferDetailsInvoiceViewBO()
        {
            TransferNCompanyInfo = new List<SMSalesTransferReportViewBO>();
            Items = new List<SMSalesTransferDetailsBO>();
            Serials = new List<SMSalesItemSerialTransferBO>();
        }
    }
}
