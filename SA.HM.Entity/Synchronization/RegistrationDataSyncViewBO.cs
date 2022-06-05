using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Synchronization
{
    public class RegistrationDataSyncViewBO
    {
        public long Id { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public virtual Guid? GuidId { get; set; }
        public bool IsSyncCompleted { get; set; }
        public string GuestName { get; set; }
        public decimal RoomRate { get; set; }
        public string RoomNumber { get; set; }
        public string DisplayArriveDate { get; set; }
        public string DisplayCheckOut { get; set; }
        public bool? IsVatAmountEnable { get; set; }
        public bool? IsServiceChargeEnable { get; set; }
        public bool? IsCityChargeEnable { get; set; }
        public bool? IsAdditionalChargeEnable { get; set; }
        public int Nights { get; set; }
        public int BillPaidBy { get; set; }
    }
}
