namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLProfitLossSetup")]
    public partial class GLProfitLossSetup
    {
        [Key]
        public int PLSetupId { get; set; }

        public int? PLHeadId { get; set; }

        public int? NodeId { get; set; }
    }
}
