namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DashboardItem")]
    public partial class DashboardItem
    {
        public long Id { get; set; }

        public int? ModuleId { get; set; }

        [StringLength(200)]
        public string ItemName { get; set; }
    }
}
