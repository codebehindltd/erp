namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollApplicantResult")]
    public partial class PayrollApplicantResult
    {
        [Key]
        public long ApplicantResultId { get; set; }

        public long JobCircularId { get; set; }

        public long ApplicantId { get; set; }

        public short InterviewTypeId { get; set; }

        public decimal MarksObtain { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
