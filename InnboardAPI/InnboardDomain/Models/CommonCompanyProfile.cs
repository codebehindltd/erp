namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCompanyProfile")]
    public partial class CommonCompanyProfile
    {
        [Key]
        public int CompanyId { get; set; }

        [StringLength(20)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(300)]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyAddress { get; set; }

        [StringLength(300)]
        public string EmailAddress { get; set; }

        [StringLength(300)]
        public string WebAddress { get; set; }

        [Required]
        [StringLength(300)]
        public string ContactNumber { get; set; }

        [StringLength(300)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string VatRegistrationNo { get; set; }

        public string Remarks { get; set; }

        public string ImageName { get; set; }

        public string ImagePath { get; set; }

        [StringLength(50)]
        public string CompanyType { get; set; }
    }
}
