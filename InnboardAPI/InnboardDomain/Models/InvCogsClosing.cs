namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvCogsClosing")]
    public partial class InvCogsClosing
    {
        [Key]
        public long CogsClosingId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CogsClosingDate { get; set; }

        public int LocationId { get; set; }

        public int CategoryId { get; set; }

        public int NodeId { get; set; }

        [Column(TypeName = "money")]
        public decimal CogsAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
