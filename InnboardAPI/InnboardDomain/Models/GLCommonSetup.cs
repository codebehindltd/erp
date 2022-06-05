namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCommonSetup")]
    public partial class GLCommonSetup
    {
        [Key]
        public int SetupId { get; set; }

        public int ProjectId { get; set; }

        [StringLength(100)]
        public string TypeName { get; set; }

        [StringLength(200)]
        public string SetupName { get; set; }

        [Column(TypeName = "text")]
        public string SetupValue { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
