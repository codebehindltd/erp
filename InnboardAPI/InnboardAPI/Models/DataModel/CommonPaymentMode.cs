namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonPaymentMode")]
    public partial class CommonPaymentMode
    {
        [Key]
        public int PaymentModeId { get; set; }

        public int? AncestorId { get; set; }

        [StringLength(100)]
        public string PaymentMode { get; set; }

        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; }

        [StringLength(5)]
        public string PaymentCode { get; set; }

        [StringLength(900)]
        public string Hierarchy { get; set; }

        public int Lvl { get; set; }

        [StringLength(900)]
        public string HierarchyIndex { get; set; }

        public int? PaymentAccountsPostingId { get; set; }

        public int? ReceiveAccountsPostingId { get; set; }

        public bool? ActiveStat { get; set; }
    }
}
