namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLFixedAssets
    {
        [Key]
        public int FixedAssetsId { get; set; }

        public int? NodeId { get; set; }

        public decimal? BlockB { get; set; }

        public decimal? BlockC { get; set; }

        public decimal? BlockD { get; set; }

        public decimal? BlockE { get; set; }

        public decimal? BlockF { get; set; }

        public decimal? BlockG { get; set; }

        public decimal? BlockH { get; set; }

        public decimal? BlockI { get; set; }
    }
}
