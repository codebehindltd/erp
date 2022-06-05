namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBillDetail")]
    public partial class RestaurantBillDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? BillId { get; set; }

        public int? KotId { get; set; }
    }
}
