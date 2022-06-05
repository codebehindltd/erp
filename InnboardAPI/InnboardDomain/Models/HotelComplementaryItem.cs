namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelComplementaryItem")]
    public partial class HotelComplementaryItem
    {
        [Key]
        public int ComplementaryItemId { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public bool? ActiveStat { get; set; }

        public bool? IsDefaultItem { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
