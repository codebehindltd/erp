namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollBestEmployeeNomination")]
    public partial class PayrollBestEmployeeNomination
    {
        [Key]
        public long BestEmpNomineeId { get; set; }

        public int DepartmentId { get; set; }

        public short Years { get; set; }

        public byte Months { get; set; }

        [Required]
        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        [StringLength(25)]
        public string Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
