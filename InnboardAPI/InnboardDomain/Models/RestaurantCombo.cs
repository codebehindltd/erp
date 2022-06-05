namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantCombo")]
    public partial class RestaurantCombo
    {
        [Key]
        public int ComboId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(250)]
        public string ComboName { get; set; }

        public decimal? ComboPrice { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ImageName { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
