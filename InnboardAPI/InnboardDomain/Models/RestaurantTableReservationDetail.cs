namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantTableReservationDetail")]
    public partial class RestaurantTableReservationDetail
    {
        [Key]
        public int ReservationDetailId { get; set; }

        public int? ReservationId { get; set; }

        public int? CostCenterId { get; set; }

        public int? TableId { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal? Amount { get; set; }

        public bool? IsRegistered { get; set; }
    }
}
