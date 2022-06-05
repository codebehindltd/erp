namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMSalesOrderDetails
    {
        [Key]
        public int DetailId { get; set; }

        public int? SOrderId { get; set; }

        public int? CostCenterId { get; set; }

        public int? StockById { get; set; }

        public int? ProductId { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Quantity { get; set; }
    }
}
