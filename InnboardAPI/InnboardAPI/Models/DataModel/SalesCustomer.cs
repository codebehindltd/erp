namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesCustomer")]
    public partial class SalesCustomer
    {
        [Key]
        public int CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string WebAddress { get; set; }

        [StringLength(200)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string ContactDesignation { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(100)]
        public string ContactEmail { get; set; }

        [StringLength(50)]
        public string ContactPhone { get; set; }

        [StringLength(50)]
        public string ContactFax { get; set; }

        [StringLength(200)]
        public string ContactPerson2 { get; set; }

        [StringLength(100)]
        public string ContactDesignation2 { get; set; }

        [StringLength(50)]
        public string Department2 { get; set; }

        [StringLength(100)]
        public string ContactEmail2 { get; set; }

        [StringLength(50)]
        public string ContactPhone2 { get; set; }

        [StringLength(50)]
        public string ContactFax2 { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
