namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestRegistration")]
    public partial class HotelGuestRegistration
    {
        [Key]
        public long GuestRegistrationId { get; set; }

        public long? RegistrationId { get; set; }

        public long? GuestId { get; set; }

        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public decimal? PaxInRate { get; set; }
    }
}
