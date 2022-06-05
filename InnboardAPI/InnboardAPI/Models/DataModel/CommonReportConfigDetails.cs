namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonReportConfigDetails
    {
        public long Id { get; set; }

        public long ReportConfigId { get; set; }

        public long NodeId { get; set; }

        [StringLength(450)]
        public string NodeName { get; set; }

        public short SortingOrder { get; set; }
    }
}
