namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetReservationClassificationDiscount")]
    public partial class BanquetReservationClassificationDiscount
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        public int? CategoryId { get; set; }

        public virtual BanquetReservation BanquetReservation { get; set; }
    }
}
