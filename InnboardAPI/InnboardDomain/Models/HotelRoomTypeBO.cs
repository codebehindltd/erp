namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class HotelRoomTypeBO
    {
        public int RoomTypeId { get; set; }
        
        public string RoomType { get; set; }
        public string TypeCode { get; set; }
        public string LocalCurrencyHead { get; set; }
        public decimal? RoomRate { get; set; }
        public decimal? RoomRateUSD { get; set; }
        public bool? ActiveStat { get; set; }
        public long? AccountsPostingHeadId { get; set; }
        public int? PaxQuantity { get; set; }
        public int? ChildQuantity { get; set; }
        public string ActiveStatus { get; set; }

    }
}
