namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmployeeBillGeneration")]
    public partial class PayrollEmployeeBillGeneration
    {
        [Key]
        public long EmployeeBillId { get; set; }

        public int EmployeeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime BillDate { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeBillNumber { get; set; }

        [StringLength(50)]
        public string ApprovedStatus { get; set; }

        [StringLength(25)]
        public string BillStatus { get; set; }

        public int? BillCurrencyId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
