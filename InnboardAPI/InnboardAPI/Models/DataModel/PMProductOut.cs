namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductOut")]
    public partial class PMProductOut
    {
        [Key]
        public int OutId { get; set; }

        [StringLength(50)]
        public string ProductOutFor { get; set; }

        public DateTime? OutDate { get; set; }

        public int RequisitionOrSalesId { get; set; }

        public int? OutFor { get; set; }

        [StringLength(25)]
        public string IssueType { get; set; }

        public long? ToCostCenterId { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(25)]
        public string IssueNumber { get; set; }

        public long? AccountPostingHeadId { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public long? ToLocationId { get; set; }

        public long? FromCostCenterId { get; set; }

        public long? FromLocationId { get; set; }
    }
}
