namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCustomFieldData")]
    public partial class CommonCustomFieldData
    {
        [Key]
        public int FieldId { get; set; }

        public string FieldType { get; set; }

        public string FieldValue { get; set; }

        public string Description { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
