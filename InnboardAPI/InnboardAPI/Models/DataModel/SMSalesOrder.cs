namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSalesOrder")]
    public partial class SMSalesOrder
    {
        [Key]
        public int SOrderId { get; set; }

        public DateTime? SODate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [StringLength(50)]
        public string SONumber { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(20)]
        public string ApprovedStatus { get; set; }

        [StringLength(50)]
        public string DeliveryStatus { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
