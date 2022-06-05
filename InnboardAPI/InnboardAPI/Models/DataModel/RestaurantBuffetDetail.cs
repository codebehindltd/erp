namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBuffetDetail")]
    public partial class RestaurantBuffetDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? BuffetId { get; set; }

        public int? ProductId { get; set; }
    }
}
