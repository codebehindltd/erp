using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelGuestBillApproved
    {

        public long ApprovedId { get; set; }

        public long? RegistrationId { get; set; }
        
        public DateTime? ServiceDate { get; set; }
        
        public DateTime? ApprovedDate { get; set; }

        [StringLength(50)]
        public string RoomType { get; set; }

        public int? RoomId { get; set; }

        [StringLength(50)]
        public string RoomNumber { get; set; }

        [StringLength(300)]
        public string ServiceName { get; set; }

        public decimal? TotalRoomCharge { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? BPPercentAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? RoomRate { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? InvoiceRackRate { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public decimal? InvoiceServiceCharge { get; set; }

        public bool? IsCitySDChargeEnable { get; set; }

        public decimal? InvoiceCitySDCharge { get; set; }

        public bool? IsVatAmountEnable { get; set; }

        public decimal? InvoiceVatAmount { get; set; }

        public bool? IsAdditionalChargeEnable { get; set; }

        public decimal? InvoiceAdditionalCharge { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public decimal? ReferenceSalesCommission { get; set; }

        public bool? IsBillHoldUp { get; set; }

        [StringLength(50)]
        public string ApprovedStatus { get; set; }

        public decimal? TotalCalculatedAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsExtraBedCharge { get; set; }
    }
}
