namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonSetup")]
    public partial class CommonSetup
    {
        [Key]
        public int SetupId { get; set; }

        [StringLength(100)]
        public string TypeName { get; set; }

        [StringLength(200)]
        public string SetupName { get; set; }

        [Column(TypeName = "text")]
        public string SetupValue { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
