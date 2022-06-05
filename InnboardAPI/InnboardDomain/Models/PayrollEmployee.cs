namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmployee")]
    public partial class PayrollEmployee
    {
        [Key]
        public int EmpId { get; set; }

        public int? AttendanceDeviceEmpId { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string EmpPassword { get; set; }

        [StringLength(20)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        public int? EmpTypeId { get; set; }

        public DateTime? JoinDate { get; set; }

        public DateTime? ProbablePFEligibilityDate { get; set; }

        public DateTime? PFEligibilityDate { get; set; }

        public DateTime? PFTerminateDate { get; set; }

        public DateTime? ProbableGratuityEligibilityDate { get; set; }

        public DateTime? GratuityEligibilityDate { get; set; }

        public DateTime? ProvisionPeriod { get; set; }

        public DateTime? InitialContractEndDate { get; set; }

        public DateTime? ResignationDate { get; set; }

        public int? DepartmentId { get; set; }

        public int? DesignationId { get; set; }

        public int? GradeId { get; set; }

        public decimal? BasicAmount { get; set; }

        public int? RepotingTo { get; set; }

        [StringLength(100)]
        public string OfficialEmail { get; set; }

        [StringLength(100)]
        public string ReferenceBy { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(100)]
        public string FathersName { get; set; }

        [StringLength(100)]
        public string MothersName { get; set; }

        public DateTime? EmpDateOfBirth { get; set; }

        [StringLength(100)]
        public string Gender { get; set; }

        [StringLength(100)]
        public string BloodGroup { get; set; }

        [StringLength(30)]
        public string Religion { get; set; }

        [StringLength(10)]
        public string Height { get; set; }

        [StringLength(20)]
        public string MaritalStatus { get; set; }

        public int? CountryId { get; set; }

        public int? DivisionId { get; set; }

        public int? DistrictId { get; set; }

        public int? ThanaId { get; set; }

        [StringLength(50)]
        public string NationalId { get; set; }

        [StringLength(50)]
        public string PassportNumber { get; set; }

        [StringLength(50)]
        public string PIssuePlace { get; set; }

        public DateTime? PIssueDate { get; set; }

        public DateTime? PExpireDate { get; set; }

        [StringLength(150)]
        public string PresentAddress { get; set; }

        [StringLength(50)]
        public string PresentCity { get; set; }

        [StringLength(20)]
        public string PresentZipCode { get; set; }

        [StringLength(50)]
        public string PresentCountry { get; set; }

        [StringLength(50)]
        public string PresentPhone { get; set; }

        [StringLength(150)]
        public string PermanentAddress { get; set; }

        [StringLength(50)]
        public string PermanentCity { get; set; }

        [StringLength(20)]
        public string PermanentZipCode { get; set; }

        [StringLength(50)]
        public string PermanentCountry { get; set; }

        [StringLength(50)]
        public string PermanentPhone { get; set; }

        [StringLength(100)]
        public string PersonalEmail { get; set; }

        public int? NodeId { get; set; }

        public int? WorkingCostCenterId { get; set; }

        public int? WorkStationId { get; set; }

        [StringLength(250)]
        public string EmergencyContactName { get; set; }

        [StringLength(50)]
        public string EmergencyContactRelationship { get; set; }

        [StringLength(50)]
        public string EmergencyContactNumber { get; set; }

        [StringLength(50)]
        public string EmergencyContactEmail { get; set; }

        public int? DonorId { get; set; }

        [StringLength(150)]
        public string ActivityCode { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        public int? CurrentLocationId { get; set; }

        [StringLength(50)]
        public string AlternativeEmail { get; set; }

        public bool? IsApplicant { get; set; }

        public bool? IsApplicantRecruitment { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool? IsProbitionary { get; set; }

        public bool? IsContacttual { get; set; }

        public int? CostCenterId { get; set; }

        public int? EmployeeStatusId { get; set; }

        public int? PayrollCurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        public int? PresentCountryId { get; set; }

        public int? PermanentCountryId { get; set; }

        public bool IsProvidentFundDeduct { get; set; }
    }
}
