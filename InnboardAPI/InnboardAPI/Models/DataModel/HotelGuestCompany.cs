namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestCompany")]
    public partial class HotelGuestCompany
    {
        [Key]
        public int CompanyId { get; set; }

        [StringLength(150)]
        public string CompanyName { get; set; }

        [StringLength(250)]
        public string CompanyAddress { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string WebAddress { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string ContactNumber { get; set; }

        [StringLength(250)]
        public string ContactDesignation { get; set; }

        [StringLength(50)]
        public string TelephoneNumber { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public decimal? DiscountPercent { get; set; }

        public int? ReferenceId { get; set; }

        public int? IndustryId { get; set; }

        public int? LocationId { get; set; }

        public int? NodeId { get; set; }

        [StringLength(50)]
        public string SignupStatus { get; set; }

        public DateTime? AffiliatedDate { get; set; }

        public decimal? CreditLimit { get; set; }

        public bool? IsMember { get; set; }

        public decimal? Balance { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
