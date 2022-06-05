namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MembershipPointDetails
    {
        public long ID { get; set; }

        public int? MemberID { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? PointWiseAmount { get; set; }

        [StringLength(50)]
        public string PointType { get; set; }

        public DateTime? TransactionDate { get; set; }

        public long? BillId { get; set; }
    }
}
