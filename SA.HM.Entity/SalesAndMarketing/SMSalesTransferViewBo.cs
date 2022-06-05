using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesTransferViewBo
    {
        public SMSalesTransferBO SMSalesTransferBO { get; set; }
        public List<SMSalesTransferDetailsBO> SMSalesTransferDetailsBOList { get; set; }
        public List<SMQuotationDetailsBO> SMQuotationDetailsBOList { get; set; }
        public List<SMSalesItemSerialTransferBO> SMSalesTransferItemSerialList { get; set; }
    }
}
