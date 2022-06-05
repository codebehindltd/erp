namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMFinishedProduct")]
    public partial class PMFinishedProduct
    {
        [Key]
        public int FinishProductId { get; set; }

        public DateTime OrderDate { get; set; }

        public int CostCenterId { get; set; }

        [StringLength(15)]
        public string ApprovedStatus { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
