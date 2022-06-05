namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserDashboardItemMapping")]
    public partial class UserDashboardItemMapping
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ItemId { get; set; }

        public int? Panel { get; set; }

        public int? Div { get; set; }
    }
}
