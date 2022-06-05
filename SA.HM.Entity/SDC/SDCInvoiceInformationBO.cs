using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SDC
{
    public class SDCInvoiceInformationBO
    {
        public int SDCInvoiceId { get; set; }
        public int BillId { get; set; }
        public string SDCInvoiceNumber { get; set; }
        public string QRCode { get; set; }
        public string BillType { get; set; }
    }
}
