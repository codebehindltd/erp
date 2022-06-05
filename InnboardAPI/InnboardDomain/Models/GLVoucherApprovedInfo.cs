namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLVoucherApprovedInfo")]
    public partial class GLVoucherApprovedInfo
    {
        [Key]
        public int ApprovedId { get; set; }
        [ForeignKey("GLLedgerMaster"),Column("DealId")]
        public long? DealId { get; set; }

        [StringLength(50)]
        public string ApprovedType { get; set; }

        public int? UserInfoId { get; set; }
        
        public virtual GLLedgerMaster GLLedgerMaster { get; set; }
    }
}
