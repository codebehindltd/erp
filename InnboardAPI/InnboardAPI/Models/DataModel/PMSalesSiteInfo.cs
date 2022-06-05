namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesSiteInfo")]
    public partial class PMSalesSiteInfo
    {
        [Key]
        public int SiteInfoId { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(20)]
        public string SiteId { get; set; }

        [StringLength(100)]
        public string SiteName { get; set; }

        [StringLength(500)]
        public string SiteAddress { get; set; }

        [StringLength(100)]
        public string SiteContactPerson { get; set; }

        [StringLength(20)]
        public string SitePhoneNumber { get; set; }

        [StringLength(100)]
        public string SiteEmail { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
