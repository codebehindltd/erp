namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSalesCallParticipant")]
    public partial class SMSalesCallParticipant
    {
        public long Id { get; set; }

        public long SalesCallEntryId { get; set; }

        [Required]
        [StringLength(50)]
        public string PrticipantType { get; set; }

        public long ContactId { get; set; }
    }
}
