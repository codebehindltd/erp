namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCashFlowSetup")]
    public partial class GLCashFlowSetup
    {
        [Key]
        public int CFSetupId { get; set; }

        public int? HeadId { get; set; }

        public int? NodeId { get; set; }
    }
}
