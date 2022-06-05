namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBillSync")]
    public partial class RestaurantBillSync
    {
        public long Id { get; set; }

        public Guid GuidId { get; set; }

        public bool IsSyncCompleted { get; set; }
    }
}
