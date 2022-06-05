namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvTransactionMode")]
    public partial class InvTransactionMode
    {
        [Key]
        public int TModeId { get; set; }

        [StringLength(300)]
        public string HeadName { get; set; }

        [Required]
        [StringLength(20)]
        public string CalculationType { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
