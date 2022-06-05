namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestServiceBillApproved")]
    public partial class HotelGuestServiceBillApproved
    {
        [Key]
        public int ApprovedId { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(50)]
        public string RoomNumber { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int ServiceBillId { get; set; }

        public DateTime? ServiceDate { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        public int? ServiceId { get; set; }

        [StringLength(100)]
        public string ServiceName { get; set; }

        public decimal? ServiceQuantity { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? InvoiceServiceRate { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public decimal? InvoiceServiceCharge { get; set; }

        public bool? IsVatAmountEnable { get; set; }

        public decimal? InvoiceVatAmount { get; set; }

        [StringLength(50)]
        public string ApprovedStatus { get; set; }

        [StringLength(50)]
        public string PaymentMode { get; set; }

        public bool? IsPaidService { get; set; }

        public bool? IsPaidServiceAchieved { get; set; }

        public bool? IsDayClosed { get; set; }

        public int? RestaurantBillId { get; set; }

        [StringLength(300)]
        public string PaidServiceAchievementStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
