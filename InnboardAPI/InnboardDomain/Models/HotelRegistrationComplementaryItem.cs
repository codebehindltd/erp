namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRegistrationComplementaryItem")]
    public partial class HotelRegistrationComplementaryItem
    {
        [Key]
        public long RCItemId { get; set; }

        public int? RegistrationId { get; set; }

        public int? ComplementaryItemId { get; set; }
    }
}
