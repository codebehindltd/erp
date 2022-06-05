namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelServiceBillTransfered")]
    public partial class HotelServiceBillTransfered
    {
        [Key]
        public int TransferedId { get; set; }

        [StringLength(50)]
        public string ModuleName { get; set; }

        public DateTime? TransferedDate { get; set; }

        public int? FromRegistrationId { get; set; }

        public int? ToRegistrationId { get; set; }

        public int? ServiceBillId { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
