namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvItemDetails
    {
        [Key]
        public int DetailId { get; set; }

        public int? ItemId { get; set; }

        public int? ItemDetailId { get; set; }

        public decimal? ItemUnit { get; set; }
    }
}
