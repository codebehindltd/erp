namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HotelCompanyContactDetails
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string TransactionType { get; set; }

        public long? TransactionId { get; set; }

        [StringLength(200)]
        public string FieldName { get; set; }

        [StringLength(200)]
        public string FieldValue { get; set; }
    }
}
