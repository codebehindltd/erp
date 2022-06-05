namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelCompanyWiseDiscountPolicy")]
    public partial class HotelCompanyWiseDiscountPolicy
    {
        [Key]
        public long CompanyWiseDiscountId { get; set; }

        public int CompanyId { get; set; }

        public int RoomTypeId { get; set; }

        [Required]
        [StringLength(15)]
        public string DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

        public bool ActiveStat { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
