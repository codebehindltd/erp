namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonBusinessPromotionDetail")]
    public partial class CommonBusinessPromotionDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? BusinessPromotionId { get; set; }

        [StringLength(100)]
        public string TransactionType { get; set; }

        public int? TransactionId { get; set; }
    }
}
