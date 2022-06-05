namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("viewChkFromLdg")]
    public partial class viewChkFromLdg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LedgerMasterId { get; set; }

        [StringLength(256)]
        public string VcheqNo { get; set; }
    }
}
