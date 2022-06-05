using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelGuestDayLetCheckOut
    {
        public int DayLetId { get; set; }

        public int? RegistrationId { get; set; }

        public decimal? RoomRate { get; set; }

        [StringLength(50)]
        public string DayLetDiscountType { get; set; }

        public decimal? DayLetDiscount { get; set; }

        public decimal? DayLetDiscountAmount { get; set; }

        [StringLength(50)]
        public string DayLetStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
