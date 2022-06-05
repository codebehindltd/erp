namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRPOUserPermission")]
    public partial class PRPOUserPermission
    {
        [Key]
        public long MappingId { get; set; }

        public int? EmpId { get; set; }

        public int? UserInfoId { get; set; }

        public int? CostCenterId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? ActiveStat { get; set; }

        public bool? IsPRAllow { get; set; }

        public bool? IsPOAllow { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
