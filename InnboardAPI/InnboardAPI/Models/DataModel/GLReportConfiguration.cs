namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLReportConfiguration")]
    public partial class GLReportConfiguration
    {
        [Key]
        public long RCId { get; set; }

        public long? AncestorId { get; set; }

        public bool? IsAccountHead { get; set; }

        public int? NodeId { get; set; }

        [StringLength(50)]
        public string NodeNumber { get; set; }

        [StringLength(256)]
        public string NodeHead { get; set; }

        public int? Lvl { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string ReportType { get; set; }

        [StringLength(100)]
        public string AccountType { get; set; }

        [StringLength(100)]
        public string CalculationType { get; set; }

        public bool? IsActiveLinkUrl { get; set; }
    }
}
