using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
   public class GuestBillSplitBO
    {
       public int BillSplitId { get; set; }
       public int RegistrationId { get; set; }
       public string BillSplitNumber { get; set; }
       public bool RoomBill { get; set; }
       public bool ServiceBill { get; set; }
       public bool IsCompanyPayment { get; set; }
       public int RoomValue { get; set; }
       public int ServiceValue { get; set; }
       public bool RoomBillIsCompany { get; set; }
       public bool ServiceBillIsCompany { get; set; }
    }
}
