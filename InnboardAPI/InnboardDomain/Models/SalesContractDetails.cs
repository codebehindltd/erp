namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SalesContractDetails
    {
        [Key]
        public int ContractDetailsId { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? SigningDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(300)]
        public string DocumentName { get; set; }

        [StringLength(500)]
        public string DocumentPath { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
