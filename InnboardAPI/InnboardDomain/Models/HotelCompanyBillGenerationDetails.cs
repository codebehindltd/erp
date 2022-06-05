namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HotelCompanyBillGenerationDetails
    {
        [Key]
        public long CompanyBillDetailsId { get; set; }

        public long CompanyBillId { get; set; }

        public long CompanyPaymentId { get; set; }

        public int BillId { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? DueAmount { get; set; }
    }
}
