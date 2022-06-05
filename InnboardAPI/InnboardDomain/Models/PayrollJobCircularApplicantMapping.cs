namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollJobCircularApplicantMapping")]
    public partial class PayrollJobCircularApplicantMapping
    {
        [Key]
        public long JobCircularApplicantMappingId { get; set; }

        public long JobCircularId { get; set; }

        public long ApplicantId { get; set; }

        [Required]
        [StringLength(10)]
        public string ApplicantType { get; set; }
    }
}
