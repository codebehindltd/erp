namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBuffet")]
    public partial class RestaurantBuffet
    {
        [Key]
        public int BuffetId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(250)]
        public string BuffetName { get; set; }

        public decimal? BuffetPrice { get; set; }

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
