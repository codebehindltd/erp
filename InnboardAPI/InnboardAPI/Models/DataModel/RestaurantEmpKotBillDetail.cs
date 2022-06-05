namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantEmpKotBillDetail")]
    public partial class RestaurantEmpKotBillDetail
    {
        [Key]
        public long DetailId { get; set; }

        public int? EmpId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        public int? KotId { get; set; }

        public int? KotDetailId { get; set; }

        public DateTime? JobStartDate { get; set; }

        public DateTime? JobEndDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(50)]
        public string JobStatus { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
