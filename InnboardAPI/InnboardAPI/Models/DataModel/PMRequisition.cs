namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMRequisition")]
    public partial class PMRequisition
    {
        [Key]
        public int RequisitionId { get; set; }

        [StringLength(20)]
        public string PRNumber { get; set; }

        public int FromCostCenterId { get; set; }

        public DateTime ReceivedByDate { get; set; }

        [StringLength(50)]
        public string RequisitionBy { get; set; }

        [StringLength(20)]
        public string ApprovedStatus { get; set; }

        [StringLength(50)]
        public string DelivaredStatus { get; set; }

        [StringLength(50)]
        public string DelivarOutStatus { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int ToCostCenterId { get; set; }
    }
}
