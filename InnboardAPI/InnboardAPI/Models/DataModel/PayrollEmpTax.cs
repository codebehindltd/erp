namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTax")]
    public partial class PayrollEmpTax
    {
        [Key]
        public int TaxCollectionId { get; set; }

        public int EmpId { get; set; }

        public decimal EmpTaxContribution { get; set; }

        public decimal CompanyTaxContribution { get; set; }

        public DateTime TaxDate { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
