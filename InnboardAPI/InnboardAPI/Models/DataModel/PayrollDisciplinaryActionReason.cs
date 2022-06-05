namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollDisciplinaryActionReason")]
    public partial class PayrollDisciplinaryActionReason
    {
        [Key]
        public int DisciplinaryActionReasonId { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionReason { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
