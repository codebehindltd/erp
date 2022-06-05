namespace InnboardAPI.Models.DataModel
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

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
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
    }
}
