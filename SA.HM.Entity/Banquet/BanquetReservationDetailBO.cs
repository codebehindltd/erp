using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationDetailBO
    {
        public long Id { get; set; }
        public long ReservationId { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemType { get; set; }        
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsComplementary { get; set; }

        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public string AdditionalChargeType { get; set; }

        public string ItemDescription { get; set; }
        public DateTime ItemArrivalTime { get; set; }

        public string ItemWiseDiscountType { get; set; }
        public decimal ItemWiseIndividualDiscount { get; set; }
        public bool IsItemEditable { get; set; }

        public string DiscountType { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }
    }
}
