namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpDistrict")]
    public partial class PayrollEmpDistrict
    {
        [Key]
        public int DistrictId { get; set; }

        public int DivisionId { get; set; }

        [Required]
        [StringLength(50)]
        public string DistrictName { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
