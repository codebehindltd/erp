namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCPayment")]
    public partial class LCPayment
    {
        [Key]
        public long PaymentId { get; set; }

        public long LCId { get; set; }

        public int? AccountHeadId { get; set; }

        public int? CurrencyId { get; set; }

        public decimal? Amount { get; set; }
    }
}
