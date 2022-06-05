namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpCareerInfo")]
    public partial class PayrollEmpCareerInfo
    {
        [Key]
        public int CareerInfoId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(500)]
        public string Objective { get; set; }

        public decimal? PresentSalary { get; set; }

        public decimal? ExpectedSalary { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        [StringLength(50)]
        public string JobLevel { get; set; }

        [StringLength(50)]
        public string AvailableType { get; set; }

        public int? PreferedJobType { get; set; }

        public int? PreferedOrganizationType { get; set; }

        [StringLength(500)]
        public string CareerSummary { get; set; }

        public int? PreferedJobLocationId { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(500)]
        public string ExtraCurriculmActivities { get; set; }
    }
}
