namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollDisciplinaryAction")]
    public partial class PayrollDisciplinaryAction
    {
        [Key]
        public long DisciplinaryActionId { get; set; }

        public int DisciplinaryActionReasonId { get; set; }

        public int EmployeeId { get; set; }

        public short ActionTypeId { get; set; }

        [StringLength(550)]
        public string ActionBody { get; set; }

        public int? ProposedActionId { get; set; }

        public DateTime? ApplicableDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
