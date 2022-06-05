using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesItemSerialTransferBO
    {
        public long SalesItemSerialTransferId { get; set; }
        public long SalesTransferId { get; set; }
        public int ItemId { get; set; }
        public string SerialNumber { get; set; }

        public string ItemName { get; set; }
    }
}
