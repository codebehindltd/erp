namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestPreferenceMapping")]
    public partial class HotelGuestPreferenceMapping
    {
        [Key]
        public long MappingId { get; set; }

        public int GuestId { get; set; }

        public long PreferenceId { get; set; }
    }
}
