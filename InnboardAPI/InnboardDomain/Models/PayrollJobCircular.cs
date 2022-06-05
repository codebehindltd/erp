namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollJobCircular")]
    public partial class PayrollJobCircular
    {
        [Key]
        public long JobCircularId { get; set; }

        public int? StaffRequisitionDetailsId { get; set; }

        [Required]
        [StringLength(50)]
        public string JobTitle { get; set; }

        public DateTime CircularDate { get; set; }

        public int JobType { get; set; }

        [Required]
        [StringLength(25)]
        public string JobLevel { get; set; }

        public int DepartmentId { get; set; }

        public short NoOfVancancie { get; set; }

        public DateTime DemandedTime { get; set; }

        public DateTime? OpenFrom { get; set; }

        public DateTime? OpenTo { get; set; }

        public byte AgeRangeFrom { get; set; }

        public byte AgeRangeTo { get; set; }

        [Required]
        [StringLength(6)]
        public string Gender { get; set; }

        public byte YearOfExperiance { get; set; }

        [Column(TypeName = "text")]
        public string JobDescription { get; set; }

        [Column(TypeName = "text")]
        public string EducationalQualification { get; set; }

        [Column(TypeName = "text")]
        public string AdditionalJobRequirement { get; set; }

        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
