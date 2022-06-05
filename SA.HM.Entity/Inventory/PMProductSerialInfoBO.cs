using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductSerialInfoBO
    {
        public int SerialId { get; set; }
        public int ProductId { get; set; }
        public int ReceivedId { get; set; }
        public int ReceiveDetailsId { get; set; }
        public int POrderId { get; set; }
        public int ItemId { get; set; }
        public string SerialNumber { get; set; }
        public int SalesId { get; set; }

        public string SerialStatus { get; set; }

        public Decimal Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }
}
