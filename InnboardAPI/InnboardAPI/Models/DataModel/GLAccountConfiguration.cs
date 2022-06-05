namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLAccountConfiguration")]
    public partial class GLAccountConfiguration
    {
        [Key]
        public int ConfigurationId { get; set; }

        [StringLength(10)]
        public string AccountType { get; set; }

        public int? ProjectId { get; set; }

        public int? NodeId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
