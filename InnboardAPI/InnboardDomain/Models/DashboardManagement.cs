namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DashboardManagement")]
    public partial class DashboardManagement
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ItemId { get; set; }

        public int? Panel { get; set; }

        [StringLength(50)]
        public string DivName { get; set; }
    }
}
