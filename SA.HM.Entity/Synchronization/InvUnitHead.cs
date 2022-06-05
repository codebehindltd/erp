namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvUnitHead
    {
        [Key]
        public int UnitHeadId { get; set; }

        [StringLength(100)]
        public string HeadName { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
