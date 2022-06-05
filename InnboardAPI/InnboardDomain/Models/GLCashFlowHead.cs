namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCashFlowHead")]
    public partial class GLCashFlowHead
    {
        [Key]
        public int HeadId { get; set; }

        public int? GroupId { get; set; }

        [StringLength(250)]
        public string CashFlowHead { get; set; }

        [StringLength(20)]
        public string NotesNumber { get; set; }

        public int? VoucherMode { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
