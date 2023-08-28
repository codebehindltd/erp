using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemMemberBasicsBO
    {
        public int MemberId { get; set; }
        public int? CompanyId { get; set; }
        public int? NodeId { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string MembershipNumber { get; set; }
        public string NameTitle { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MemberName { get; set; }
        public int? MemberGender { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MemberAddress { get; set; }
        public string ResidencePhone { get; set; }
        public string OfficePhone { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficeEmail { get; set; }
        public string HomeFax { get; set; }
        public string OfficeFax { get; set; }
        public int? MaritalStatus { get; set; }
        public int? BloodGroup { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ExpiryDateInfo { get; set; }
        public string MailAddress { get; set; }
        public int? Nationality { get; set; }
        public string PassportNumber { get; set; }
        public string Organization { get; set; }
        public string Occupation { get; set; }
        public string Designation { get; set; }
        public decimal? AnnualTurnover { get; set; }
        public decimal? MonthlyIncome { get; set; }
        public decimal? SecurityDeposit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string MaritalSt { get; set; }
        public string MemberGenderSt { get; set; }
        public string NationalitySt { get; set; }
        public string BloodGroupName { get; set; }
        public decimal PointWiseAmount { get; set; }
        public decimal AchievePoint { get; set; }
        public decimal DiscountPercent { get; set; }
        public string MemberIdNDiscount { get; set; }
        public string NameWithMembershipNumber { get; set; }
        public int DepartmentId { get; set; }
        public int AttendanceDeviceMemberId { get; set; }
        public string OfficeAddress { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? ReligionId { get; set; }
        public string Religion { get; set; }
        public string NomineeName { get; set; }
        public string NomineeFather { get; set; }
        public string NomineeMother { get; set; }
        public DateTime? NomineeDOB { get; set; }
        public string NomineeRelation { get; set; }
        public int? NomineeRelationId { get; set; }
        public string NationalID { get; set; }
        public decimal Balance { get; set; }
        public string MemberPassword { get; set; }

    }
}
