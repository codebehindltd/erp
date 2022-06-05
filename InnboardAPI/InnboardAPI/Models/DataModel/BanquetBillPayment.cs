namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetBillPayment")]
    public partial class BanquetBillPayment
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? PaymentAmount { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public long? DealId { get; set; }

        public virtual BanquetReservation BanquetReservation { get; set; }
    }
}
