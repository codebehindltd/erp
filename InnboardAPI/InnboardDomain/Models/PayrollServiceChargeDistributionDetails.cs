namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollServiceChargeDistributionDetails
    {
        [Key]
        public long ServiceProcessDetailsId { get; set; }

        public long ServiceProcessId { get; set; }

        public int EmpId { get; set; }

        public byte TotalAttendance { get; set; }

        public byte ServiceDays { get; set; }

        public decimal? DistributionPercentage { get; set; }

        public decimal ServiceAmount { get; set; }
    }
}
