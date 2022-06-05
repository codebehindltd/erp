namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLFiscalYearClosingDetails
    {
        [Key]
        public long ClosingBalanceId { get; set; }

        public long? YearClosingId { get; set; }

        public int FiscalYearId { get; set; }

        public int CompanyId { get; set; }

        public int ProjectId { get; set; }

        public int? DonorId { get; set; }

        public long NodeId { get; set; }

        [Required]
        [StringLength(250)]
        public string NodeHead { get; set; }

        [Column(TypeName = "money")]
        public decimal ClosingDRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal ClosingCRAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal ClosingBalance { get; set; }
    }
}
