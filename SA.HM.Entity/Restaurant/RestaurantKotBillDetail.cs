namespace HotelManagement.Entity.Restaurant
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class RestaurantKotBillDetail
    {
        public int KotDetailId { get; set; }

        public int? KotId { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; }

        public int? ItemId { get; set; }

        [StringLength(300)]
        public string ItemName { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? UnitRate { get; set; }

        public decimal? Amount { get; set; }

        public decimal? ItemTotalAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public bool? PrintFlag { get; set; }

        public bool? IsChanged { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsDispatch { get; set; }

        public decimal? ItemCost { get; set; }

        public int? EmpId { get; set; }

        [StringLength(50)]
        public string DeliveryStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool? IsItemReturn { get; set; }
        public decimal? ReturnQuantity { get; set; }
    }
}
