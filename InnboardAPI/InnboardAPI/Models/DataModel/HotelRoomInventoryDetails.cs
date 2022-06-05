namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HotelRoomInventoryDetails
    {
        [Key]
        public int OutDetailsId { get; set; }

        public int InventoryOutId { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        public int StockById { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal? AverageCost { get; set; }
    }
}
