namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantToken")]
    public partial class RestaurantToken
    {
        [Key]
        public long TokenId { get; set; }

        public int CostCenterId { get; set; }

        public int BearerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TokenDate { get; set; }

        public int Token { get; set; }

        [Required]
        [StringLength(15)]
        public string TokenNumber { get; set; }

        public int? KotId { get; set; }

        public int? BillId { get; set; }

        public bool? IsBillHoldup { get; set; }

        [StringLength(25)]
        public string TokenStatus { get; set; }
    }
}
