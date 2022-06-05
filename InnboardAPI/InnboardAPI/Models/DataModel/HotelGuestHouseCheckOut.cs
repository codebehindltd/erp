namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestHouseCheckOut")]
    public partial class HotelGuestHouseCheckOut
    {
        [Key]
        public int CheckOutId { get; set; }

        public int? RegistrationId { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        public DateTime? CheckOutDate { get; set; }

        [StringLength(20)]
        public string PayMode { get; set; }

        public int? BankId { get; set; }

        [StringLength(250)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string ChecqueNumber { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(20)]
        public string CardType { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(256)]
        public string CardHolderName { get; set; }

        [StringLength(50)]
        public string CardReference { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        [StringLength(500)]
        public string RebateRemarks { get; set; }

        public int? BillPaidBy { get; set; }

        public int? DealId { get; set; }

        public bool? IsDayClosed { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
