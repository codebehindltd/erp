namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpLastMonthBenifitsPayment")]
    public partial class PayrollEmpLastMonthBenifitsPayment
    {
        [Key]
        public int BenifitId { get; set; }

        public int EmpId { get; set; }

        public decimal? AfterServiceBenefit { get; set; }

        public decimal? EmployeePFContribution { get; set; }

        public decimal? CompanyPFContribution { get; set; }

        public decimal? LeaveBalanceDays { get; set; }

        public decimal? LeaveBalanceAmount { get; set; }

        public int? ProcessYear { get; set; }

        public DateTime? ProcessDateFrom { get; set; }

        public DateTime? ProcessDateTo { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
