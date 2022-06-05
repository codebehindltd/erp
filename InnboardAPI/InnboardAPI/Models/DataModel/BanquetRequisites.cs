namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BanquetRequisites
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        public bool? ActiveStat { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public virtual GLNodeMatrix GLNodeMatrix { get; set; }
    }
}
