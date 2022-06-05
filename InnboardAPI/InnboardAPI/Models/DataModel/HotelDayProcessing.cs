namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelDayProcessing")]
    public partial class HotelDayProcessing
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string ProcessType { get; set; }

        public DateTime? ProcessDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
