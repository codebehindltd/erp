namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelGuestInformationOnline")]
    public partial class HotelGuestInformationOnline
    {
        [Key]
        public int GuestId { get; set; }

        [StringLength(20)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string GuestName { get; set; }

        public DateTime? GuestDOB { get; set; }

        [StringLength(20)]
        public string GuestSex { get; set; }

        [StringLength(50)]
        public string GuestEmail { get; set; }

        public int? ProfessionId { get; set; }

        [StringLength(50)]
        public string GuestPhone { get; set; }

        [StringLength(250)]
        public string GuestAddress1 { get; set; }

        [StringLength(250)]
        public string GuestAddress2 { get; set; }

        [StringLength(50)]
        public string GuestCity { get; set; }

        [StringLength(10)]
        public string GuestZipCode { get; set; }

        public int? GuestCountryId { get; set; }

        [StringLength(50)]
        public string GuestNationality { get; set; }

        [StringLength(50)]
        public string GuestDrivinlgLicense { get; set; }

        [StringLength(50)]
        public string GuestAuthentication { get; set; }

        [StringLength(50)]
        public string NationalId { get; set; }

        [StringLength(50)]
        public string PassportNumber { get; set; }

        public DateTime? PIssueDate { get; set; }

        [StringLength(50)]
        public string PIssuePlace { get; set; }

        public DateTime? PExpireDate { get; set; }

        [StringLength(50)]
        public string VisaNumber { get; set; }

        public DateTime? VIssueDate { get; set; }

        public DateTime? VExpireDate { get; set; }

        [Column(TypeName = "text")]
        public string GuestPreferences { get; set; }
    }
}
