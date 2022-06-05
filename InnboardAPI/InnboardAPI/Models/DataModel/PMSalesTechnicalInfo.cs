namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesTechnicalInfo")]
    public partial class PMSalesTechnicalInfo
    {
        [Key]
        public int TechnicalInfoId { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(100)]
        public string TechnicalContactPerson { get; set; }

        [StringLength(100)]
        public string TechnicalPersonDepartment { get; set; }

        [StringLength(100)]
        public string TechnicalPersonDesignation { get; set; }

        [StringLength(100)]
        public string TechnicalPersonPhone { get; set; }

        [StringLength(100)]
        public string TechnicalPersonEmail { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
