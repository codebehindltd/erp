namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesBillingInfo")]
    public partial class PMSalesBillingInfo
    {
        [Key]
        public int BillingInfoId { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(100)]
        public string BillingContactPerson { get; set; }

        [StringLength(100)]
        public string BillingPersonDepartment { get; set; }

        [StringLength(100)]
        public string BillingPersonDesignation { get; set; }

        [StringLength(100)]
        public string BillingPersonPhone { get; set; }

        [StringLength(100)]
        public string BillingPersonEmail { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
