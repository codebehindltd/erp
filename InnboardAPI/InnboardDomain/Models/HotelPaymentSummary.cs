namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelPaymentSummary")]
    public partial class HotelPaymentSummary
    {
        [Key]
        public long SummaryId { get; set; }

        public DateTime? TransactionDate { get; set; }

        [StringLength(100)]
        public string ServiceDate { get; set; }

        [StringLength(100)]
        public string RoomNumber { get; set; }

        [StringLength(100)]
        public string BillNumber { get; set; }

        [StringLength(300)]
        public string PaymentDescription { get; set; }

        [StringLength(200)]
        public string PaymentMode { get; set; }

        [StringLength(300)]
        public string POSTerminalBank { get; set; }

        public decimal? ReceivedAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        [StringLength(300)]
        public string OperatedBy { get; set; }

        [StringLength(100)]
        public string ReportType { get; set; }
    }
}
