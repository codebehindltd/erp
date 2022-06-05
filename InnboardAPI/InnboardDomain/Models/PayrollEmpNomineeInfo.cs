namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpNomineeInfo")]
    public partial class PayrollEmpNomineeInfo
    {
        [Key]
        public int NomineeId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(200)]
        public string NomineeName { get; set; }

        [StringLength(100)]
        public string Relationship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string Age { get; set; }

        public decimal? Percentage { get; set; }
    }
}
