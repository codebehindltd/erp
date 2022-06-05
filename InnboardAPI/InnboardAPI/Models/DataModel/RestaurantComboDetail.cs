namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantComboDetail")]
    public partial class RestaurantComboDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? ComboId { get; set; }

        public int? ProductId { get; set; }

        public decimal? ProductUnit { get; set; }
    }
}
