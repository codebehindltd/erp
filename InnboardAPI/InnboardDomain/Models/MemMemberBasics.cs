namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MemMemberBasics
    {
        [Key]
        public int MemberId { get; set; }

        public int? CompanyId { get; set; }

        public int? NodeId { get; set; }

        public int? TypeId { get; set; }

        [StringLength(50)]
        public string MembershipNumber { get; set; }

        [StringLength(50)]
        public string NameTitle { get; set; }

        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [StringLength(200)]
        public string LastName { get; set; }

        public int MemberGender { get; set; }

        [StringLength(256)]
        public string FatherName { get; set; }

        [StringLength(256)]
        public string MotherName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500)]
        public string MemberAddress { get; set; }

        [StringLength(256)]
        public string ResidencePhone { get; set; }

        [StringLength(256)]
        public string OfficePhone { get; set; }

        [StringLength(256)]
        public string MobileNumber { get; set; }

        [StringLength(256)]
        public string PersonalEmail { get; set; }

        [StringLength(256)]
        public string OfficeEmail { get; set; }

        [StringLength(256)]
        public string HomeFax { get; set; }

        [StringLength(256)]
        public string OfficeFax { get; set; }

        public int MaritalStatus { get; set; }

        public int BloodGroup { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string MailAddress { get; set; }

        public int? Nationality { get; set; }

        [StringLength(256)]
        public string PassportNumber { get; set; }

        [StringLength(256)]
        public string Organization { get; set; }

        [StringLength(256)]
        public string Occupation { get; set; }

        [StringLength(256)]
        public string Designation { get; set; }

        public decimal? AnnualTurnover { get; set; }

        public decimal? MonthlyIncome { get; set; }

        public decimal SecurityDeposit { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        public decimal AchievePoint { get; set; }

        [StringLength(200)]
        public string NickName { get; set; }

        public int? CountryId { get; set; }

        public int? ReligionId { get; set; }

        public int? ProfessionId { get; set; }

        public int? Introducer_1_id { get; set; }

        public int? Introducer_2_id { get; set; }

        [StringLength(200)]
        public string Hobbies { get; set; }

        [StringLength(200)]
        public string Awards { get; set; }

        [StringLength(200)]
        public string OfficeAddress { get; set; }

        [StringLength(200)]
        public string NameBangla { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [StringLength(200)]
        public string NationalitySt { get; set; }

        public bool? IsAccepted { get; set; }

        public bool? IsRejected { get; set; }

        public bool? IsDeferred { get; set; }

        [StringLength(200)]
        public string Remarks1 { get; set; }

        [StringLength(200)]
        public string Remarks2 { get; set; }

        [StringLength(200)]
        public string BirthPlace { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }

        [StringLength(100)]
        public string NationalID { get; set; }

        public bool? IsAccepted1 { get; set; }

        public bool? IsRejected1 { get; set; }

        public bool? IsDeferred1 { get; set; }

        public bool? IsAccepted2 { get; set; }

        public bool? IsRejected2 { get; set; }

        public bool? IsDeferred2 { get; set; }

        [StringLength(200)]
        public string NomineeName { get; set; }

        [StringLength(200)]
        public string NomineeFather { get; set; }

        [StringLength(200)]
        public string NomineeMother { get; set; }

        public DateTime? NomineeDOB { get; set; }

        public int? NomineeRelationId { get; set; }

        [StringLength(200)]
        public string PathPersonalImg { get; set; }

        [StringLength(200)]
        public string PathNIdImage { get; set; }

        public DateTime? MeetingDate { get; set; }

        public DateTime? MeetingDateEC { get; set; }

        [StringLength(200)]
        public string MeetingDecision { get; set; }

        [StringLength(200)]
        public string MeetingDecisionEC { get; set; }

        public string TransactionType { get; set; }
        public string TransactionId { get; set; }
        public string TransactionDetails { get; set; }
        public string MemberAppsProfilePicture { get; set; }
        public virtual byte[] MemberAppsProfilePictureByte { get; set; }
    }
}
