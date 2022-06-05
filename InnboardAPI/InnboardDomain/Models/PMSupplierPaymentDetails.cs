namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMSupplierPaymentDetails
    {
        [Key]
        public long PaymentDetailsId { get; set; }

        public long PaymentId { get; set; }

        public long SupplierPaymentId { get; set; }

        public long BillId { get; set; }

        [Column(TypeName = "money")]
        public decimal PaymentAmount { get; set; }
    }
}
