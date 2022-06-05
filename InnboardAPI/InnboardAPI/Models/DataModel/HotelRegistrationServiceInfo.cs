namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelRegistrationServiceInfo")]
    public partial class HotelRegistrationServiceInfo
    {
        [Key]
        public int DetailServiceId { get; set; }

        public int? RegistrationId { get; set; }

        public int? ServiceId { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool? IsAchieved { get; set; }
    }
}
