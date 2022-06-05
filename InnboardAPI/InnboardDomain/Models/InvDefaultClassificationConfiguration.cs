namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvDefaultClassificationConfiguration")]
    public partial class InvDefaultClassificationConfiguration
    {
        [Key]
        public int ConfigurationId { get; set; }

        public int? ClassificationId { get; set; }
    }
}
