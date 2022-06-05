namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemMemberReference")]
    public partial class MemMemberReference
    {
        [Key]
        public int ReferenceId { get; set; }

        public int? MemberId { get; set; }

        [StringLength(200)]
        public string Arbitrator { get; set; }

        [StringLength(50)]
        public string ArbitratorMode { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
