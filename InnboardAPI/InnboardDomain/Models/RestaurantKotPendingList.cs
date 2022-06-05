namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantKotPendingList")]
    public partial class RestaurantKotPendingList
    {
        [Key]
        public long KotPendingListId { get; set; }

        public int? TableId { get; set; }

        public long KotId { get; set; }

        public DateTime KotDate { get; set; }
    }
}
