namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLNotesConfiguration")]
    public partial class GLNotesConfiguration
    {
        [Key]
        public int ConfigurationId { get; set; }

        [StringLength(50)]
        public string ConfigurationType { get; set; }

        [StringLength(20)]
        public string NotesNumber { get; set; }

        public int? NodeId { get; set; }
    }
}
