namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollStaffRequisitionDetails
    {
        [Key]
        public long StaffRequisitionDetailsId { get; set; }

        public long StaffRequisitionId { get; set; }

        public int JobType { get; set; }

        [Required]
        [StringLength(25)]
        public string JobLevel { get; set; }

        public short RequisitionQuantity { get; set; }

        public decimal SalaryAmount { get; set; }

        public DateTime? DemandDate { get; set; }

        public int? FiscalYear { get; set; }
    }
}
