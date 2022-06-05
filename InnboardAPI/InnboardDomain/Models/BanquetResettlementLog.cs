namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetResettlementLog")]
    public partial class BanquetResettlementLog
    {
        [Key]
        public long ResettlementHistoryId { get; set; }

        public long ReservationId { get; set; }

        public int CostCenterId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReservationNumber { get; set; }

        public DateTime ResettlementDate { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(300)]
        public string ItemName { get; set; }

        public decimal ItemUnit { get; set; }

        public decimal UnitRate { get; set; }

        public decimal Amount { get; set; }

        public decimal CalculatedDiscountAmount { get; set; }

        public decimal AverageCost { get; set; }

        public int CreatedBy { get; set; }
    }
}
