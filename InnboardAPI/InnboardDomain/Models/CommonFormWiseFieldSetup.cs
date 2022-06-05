namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonFormWiseFieldSetup")]
    public partial class CommonFormWiseFieldSetup
    {
        public int Id { get; set; }

        public int PageId { get; set; }

        [Required]
        [StringLength(200)]
        public string FieldId { get; set; }

        [Required]
        [StringLength(400)]
        public string FieldName { get; set; }

        public bool IsMandatory { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
