using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
   public class HotelGuestDayLetCheckOutBO
    {
        public int DayLetId { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationIdList { get; set; }

        public decimal RoomRate { get; set; }
        public string DayLetDiscountType { get; set; }
        public decimal DayLetDiscount { get; set; }
        public decimal DayLetDiscountAmount { get; set; }
        public string DayLetStatus { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
