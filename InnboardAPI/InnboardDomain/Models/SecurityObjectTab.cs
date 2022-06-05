namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityObjectTab")]
    public partial class SecurityObjectTab
    {
        [Key]
        public int ObjectTabId { get; set; }

        public int? ModuleId { get; set; }

        [Required]
        [StringLength(50)]
        public string ObjectGroupHead { get; set; }

        [Required]
        [StringLength(256)]
        public string ObjectHead { get; set; }

        [Required]
        [StringLength(256)]
        public string MenuHead { get; set; }

        [Required]
        [StringLength(20)]
        public string ObjectType { get; set; }

        [Required]
        [StringLength(256)]
        public string FormName { get; set; }

        public bool ActiveStat { get; set; }
    }
}
