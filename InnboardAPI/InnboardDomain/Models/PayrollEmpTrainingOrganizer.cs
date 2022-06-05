namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTrainingOrganizer")]
    public partial class PayrollEmpTrainingOrganizer
    {
        [Key]
        public int OrganizerId { get; set; }

        [StringLength(50)]
        public string TrainingType { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganizerName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string ContactNo { get; set; }
    }
}
