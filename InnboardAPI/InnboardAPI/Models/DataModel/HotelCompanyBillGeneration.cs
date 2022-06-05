namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelCompanyBillGeneration")]
    public partial class HotelCompanyBillGeneration
    {
        [Key]
        public long CompanyBillId { get; set; }

        public int CompanyId { get; set; }

        [Column(TypeName = "date")]
        public DateTime BillDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyBillNumber { get; set; }

        [StringLength(50)]
        public string ApprovedStatus { get; set; }

        [StringLength(25)]
        public string BillStatus { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? BillCurrencyId { get; set; }
    }
}
