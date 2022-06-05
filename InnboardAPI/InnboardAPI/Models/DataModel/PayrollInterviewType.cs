namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollInterviewType")]
    public partial class PayrollInterviewType
    {
        [Key]
        public short InterviewTypeId { get; set; }

        [Required]
        [StringLength(150)]
        public string InterviewName { get; set; }

        public decimal Marks { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
