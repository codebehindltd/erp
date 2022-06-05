namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantReservationItemDetail")]
    public partial class RestaurantReservationItemDetail
    {
        [Key]
        public int ItemDetailId { get; set; }

        public int? ReservationId { get; set; }

        public int? ItemTypeId { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; }

        public int? ItemId { get; set; }

        [StringLength(300)]
        public string ItemName { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice { get; set; }

        public bool? IsComplementary { get; set; }
    }
}
