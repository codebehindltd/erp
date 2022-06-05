namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelReservationComplementaryItem")]
    public partial class HotelReservationComplementaryItem
    {
        [Key]
        public long RCItemId { get; set; }

        public int? ReservationId { get; set; }

        public int? ComplementaryItemId { get; set; }
    }
}
