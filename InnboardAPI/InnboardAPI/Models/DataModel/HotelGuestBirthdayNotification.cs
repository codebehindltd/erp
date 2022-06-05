namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestBirthdayNotification")]
    public partial class HotelGuestBirthdayNotification
    {
        public long Id { get; set; }

        public long GuestId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public bool IsEmailSent { get; set; }

        public bool IsSmsSent { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
