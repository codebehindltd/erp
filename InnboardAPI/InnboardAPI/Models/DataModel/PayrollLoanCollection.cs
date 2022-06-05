namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollLoanCollection")]
    public partial class PayrollLoanCollection
    {
        [Key]
        public int CollectionId { get; set; }

        public int LoanId { get; set; }

        public int EmpId { get; set; }

        public int InstallmentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime CollectionDate { get; set; }

        public decimal CollectionAmount { get; set; }

        public decimal InterestCollectionAmount { get; set; }

        public decimal? LoanBalance { get; set; }

        public decimal? InterestBalance { get; set; }

        [StringLength(15)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
