namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLReportConfigurationDetail")]
    public partial class GLReportConfigurationDetail
    {
        [Key]
        public int RCDetailId { get; set; }

        public int? RCId { get; set; }

        public int? NodeId { get; set; }
    }
}
