using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantTableReservationBO
    {
        public long ReservationId { get; set; }
        public string ReservationNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public string ReservedCompany { get; set; }
        public long GuestId { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string ContactEmail { get; set; }
        public int TotalTableNumber { get; set; }
        public string ReservedMode { get; set; }
        public string ReservationType { get; set; }
        public string ReservationMode { get; set; }
        public DateTime PendingDeadline { get; set; }
        public bool IsListedCompany { get; set; }
        public int CompanyId { get; set; }
        public int BusinessPromotionId { get; set; }
        public int ReferenceId { get; set; }
        public string PaymentMode { get; set; }
        public int PayFor { get; set; }
        public int CurrencyType { get; set; }
        public decimal ConversionRate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string DiscountType { get; set; }
        public decimal Amount { get; set; }
    }
}
