using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class BirthdayNotificationViewBO
    {
        public string GuestName { get; set; }
        public string GuestDOB { get; set; }
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestEmail { get; set; }

        public Int64? Id { get; set; }
        public Int64? GuestId { get; set; }
        public DateTime? Date { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsSmsSent { get; set; }

        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
