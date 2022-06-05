namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCashFlowGroupHead")]
    public partial class GLCashFlowGroupHead
    {
        [Key]
        public int GroupId { get; set; }

        [StringLength(250)]
        public string GroupHead { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
