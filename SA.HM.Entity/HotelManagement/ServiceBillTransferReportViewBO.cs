using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ServiceBillTransferReportViewBO
    {
        public string TransferedDate { get; set; }
        public int ServiceBillId { get; set; }
        public decimal ServiceRate { get; set; }
        public string ServiceName { get; set; }
        public string FromRoomNumber { get; set; }
        public string ToRoomNumber { get; set; }
        public string TransferedBy { get; set; }
        public string BillNumber { get; set; }
        public string Remarks { get; set; }
    }
}
