namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SalesServiceBundleDetails
    {
        [Key]
        public int DetailsId { get; set; }

        public int? BundleId { get; set; }

        [StringLength(50)]
        public string IsProductOrService { get; set; }

        public int? ProductId { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
