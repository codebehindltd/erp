namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvUnitConversion
    {
        [Key]
        public int ConversionId { get; set; }

        public int? FromUnitHeadId { get; set; }

        public int? ToUnitHeadId { get; set; }

        public decimal? ConversionRate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
