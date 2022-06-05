namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBillClassificationDiscount")]
    public partial class RestaurantBillClassificationDiscount
    {
        [Key]
        public int DiscountId { get; set; }

        public int? BillId { get; set; }

        public int? ClassificationId { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountAmount { get; set; }
    }
}
