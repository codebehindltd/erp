namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLProfitLossHead")]
    public partial class GLProfitLossHead
    {
        [Key]
        public int PLHeadId { get; set; }

        public int? GroupId { get; set; }

        [StringLength(250)]
        public string PLHead { get; set; }

        [StringLength(20)]
        public string NotesNumber { get; set; }

        [StringLength(20)]
        public string CalculationMode { get; set; }

        [StringLength(20)]
        public string DisplayMode { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
