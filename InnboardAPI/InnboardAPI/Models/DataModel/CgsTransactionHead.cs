namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CgsTransactionHead")]
    public partial class CgsTransactionHead
    {
        [Key]
        public int TransactionHeadId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        public bool ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        [StringLength(20)]
        public string CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [StringLength(20)]
        public string LastModifiedDate { get; set; }
    }
}
