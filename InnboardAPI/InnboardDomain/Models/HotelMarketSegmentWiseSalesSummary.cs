namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelMarketSegmentWiseSalesSummary")]
    public partial class HotelMarketSegmentWiseSalesSummary
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int MarketSegmentId { get; set; }

        public int ReferenceId { get; set; }

        public int Room { get; set; }

        public decimal RoomRate { get; set; }

        public int TotalRoom { get; set; }

        public int Pax { get; set; }

        public int MaxPax { get; set; }
    }
}
