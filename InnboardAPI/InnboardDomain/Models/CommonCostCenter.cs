namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCostCenter")]
    public partial class CommonCostCenter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int CostCenterId { get; set; }

        public int CompanyId { get; set; }

        public int? AccountsPostingHeadId { get; set; }

        [Required]
        [StringLength(256)]
        public string CostCenter { get; set; }

        [StringLength(256)]
        public string CostCenterLogo { get; set; }

        [StringLength(2)]
        public string BillNumberPrefix { get; set; }

        public bool? IsServiceChargeEnable { get; set; }

        public decimal? ServiceCharge { get; set; }

        public bool? IsCitySDChargeEnable { get; set; }

        public decimal? CitySDCharge { get; set; }

        public bool? IsVatEnable { get; set; }

        public decimal? VatAmount { get; set; }

        public int? IsVatSChargeInclusive { get; set; }

        public int? IsRatePlusPlus { get; set; }

        public bool? IsAdditionalChargeEnable { get; set; }

        [StringLength(50)]
        public string AdditionalChargeType { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public bool? IsVatOnSDCharge { get; set; }

        [StringLength(50)]
        public string CostCenterType { get; set; }

        public bool? IsRestaurant { get; set; }

        public decimal? ReadingNumber { get; set; }

        public int? OutletType { get; set; }

        [StringLength(50)]
        public string DefaultView { get; set; }

        public int? DefaultStockLocationId { get; set; }

        public bool? IsDefaultCostCenter { get; set; }

        public int? InvoiceTemplate { get; set; }

        public int? BillingStartTime { get; set; }

        public bool? IsDiscountApplicableOnRackRate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? PayrollDeptId { get; set; }
    }
}
