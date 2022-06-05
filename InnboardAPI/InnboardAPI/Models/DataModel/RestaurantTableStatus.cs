namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RestaurantTableStatus
    {
        [Key]
        public int StatusId { get; set; }

        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
