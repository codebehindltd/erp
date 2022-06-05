namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSalesCallEntry")]
    public partial class SMSalesCallEntry
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LogType { get; set; }

        public DateTime? MeetingDate { get; set; }

        [StringLength(150)]
        public string MeetingLocation { get; set; }

        [StringLength(350)]
        public string ParticipantFromParty { get; set; }

        [StringLength(150)]
        public string MeetingAgenda { get; set; }

        [StringLength(500)]
        public string Decission { get; set; }

        [StringLength(150)]
        public string MeetingAfterAction { get; set; }

        [StringLength(50)]
        public string EmailType { get; set; }

        [StringLength(50)]
        public string CallStatus { get; set; }

        [StringLength(550)]
        public string LogBody { get; set; }

        public int? CompanyId { get; set; }

        public long? DealId { get; set; }

        public long? ContactId { get; set; }

        public DateTime LogDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
