namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemSupplierMapping")]
    public partial class InvItemSupplierMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? SupplierId { get; set; }

        public int? ItemId { get; set; }
    }
}
