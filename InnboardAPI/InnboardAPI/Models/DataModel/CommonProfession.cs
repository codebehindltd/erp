namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonProfession")]
    public partial class CommonProfession
    {
        [Key]
        public int ProfessionId { get; set; }

        [StringLength(200)]
        public string ProfessionName { get; set; }

        [StringLength(20)]
        public string ProfessionCode { get; set; }
    }
}
