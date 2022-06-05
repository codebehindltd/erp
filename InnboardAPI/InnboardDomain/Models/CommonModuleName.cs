namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonModuleName")]
    public partial class CommonModuleName
    {
        [Key]
        public int ModuleId { get; set; }

        public int? TypeId { get; set; }

        [StringLength(300)]
        public string ModuleName { get; set; }

        [StringLength(300)]
        public string GroupName { get; set; }

        [StringLength(300)]
        public string ModulePath { get; set; }

        public bool? IsReportType { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
