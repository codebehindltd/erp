namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLAccountsMapping")]
    public partial class GLAccountsMapping
    {
        [Key]
        public int MappingId { get; set; }

        [StringLength(500)]
        public string MappingKey { get; set; }

        [StringLength(200)]
        public string MappingValue { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
