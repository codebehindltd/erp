namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBearer")]
    public partial class RestaurantBearer
    {
        [Key]
        public int BearerId { get; set; }

        public int? UserInfoId { get; set; }

        [StringLength(50)]
        public string BearerPassword { get; set; }

        public int? CostCenterId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? IsBearer { get; set; }

        public bool IsRestaurantBillCanSettle { get; set; }

        public bool? IsItemCanEditDelete { get; set; }

        public bool? IsItemSearchEnable { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
