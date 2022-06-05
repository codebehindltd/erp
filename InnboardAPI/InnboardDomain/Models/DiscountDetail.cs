namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountDetail")]
    public partial class DiscountDetail
    {
        public long Id { get; set; }

        public long DiscountMasterId { get; set; }

        public long DiscountForId { get; set; }

        [StringLength(25)]
        public string DiscountType { get; set; }

        public decimal Discount { get; set; }

        public virtual DiscountMaster DiscountMaster { get; set; }
    }
}
