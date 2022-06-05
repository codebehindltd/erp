namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonReportConfigMaster")]
    public partial class CommonReportConfigMaster
    {
        public long Id { get; set; }

        public int ReportTypeId { get; set; }

        public long? AncestorId { get; set; }

        [Required]
        [StringLength(350)]
        public string Caption { get; set; }

        public short SortingOrder { get; set; }

        public long? Lvl { get; set; }

        public string Hierarchy { get; set; }

        public long? HierarchyIndex { get; set; }

        public bool? IsParent { get; set; }

        [StringLength(50)]
        public string NodeType { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
