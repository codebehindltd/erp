namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestReference")]
    public partial class HotelGuestReference
    {
        [Key]
        public int ReferenceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(150)]
        public string Organization { get; set; }

        [StringLength(150)]
        public string Designation { get; set; }

        [StringLength(50)]
        public string TelephoneNumber { get; set; }

        [StringLength(50)]
        public string CellNumber { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public decimal? SalesCommission { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
