namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLRuleBreak")]
    public partial class GLRuleBreak
    {
        [Key]
        public int RuleBreakId { get; set; }

        public int? NodeId { get; set; }
    }
}
