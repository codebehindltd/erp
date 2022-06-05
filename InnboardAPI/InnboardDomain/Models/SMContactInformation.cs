namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMContactInformation")]
    public partial class SMContactInformation
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNo { get; set; }

        public int? ContactOwnerId { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime? LastContactDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? LifeCycleId { get; set; }

        public long? SourceId { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? DateAnniversary { get; set; }

        [StringLength(200)]
        public string PersonalAddress { get; set; }

        [StringLength(50)]
        public string ContactType { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(50)]
        public string TicketNo { get; set; }

        [StringLength(50)]
        public string MobilePersonal { get; set; }

        [StringLength(50)]
        public string MobileWork { get; set; }

        [StringLength(50)]
        public string PhonePersonal { get; set; }

        [StringLength(50)]
        public string PhoneWork { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Skype { get; set; }

        [StringLength(100)]
        public string Whatsapp { get; set; }

        [StringLength(100)]
        public string Twitter { get; set; }

        [StringLength(100)]
        public string EmailWork { get; set; }
    }
}
