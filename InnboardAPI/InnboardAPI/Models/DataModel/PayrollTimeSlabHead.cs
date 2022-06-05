namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollTimeSlabHead")]
    public partial class PayrollTimeSlabHead
    {
        [Key]
        public int TimeSlabId { get; set; }

        [StringLength(50)]
        public string TimeSlabHead { get; set; }

        public DateTime? SlabStartTime { get; set; }

        public DateTime? SlabEndTime { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
