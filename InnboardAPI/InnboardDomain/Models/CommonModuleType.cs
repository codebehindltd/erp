namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonModuleType")]
    public partial class CommonModuleType
    {
        [Key]
        public int TypeId { get; set; }

        [StringLength(300)]
        public string ModuleType { get; set; }
    }
}
