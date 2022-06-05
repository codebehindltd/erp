namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelReservationNoShowProcess")]
    public partial class HotelReservationNoShowProcess
    {
        [Key]
        public int ProcessId { get; set; }

        public DateTime? ProcessDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
