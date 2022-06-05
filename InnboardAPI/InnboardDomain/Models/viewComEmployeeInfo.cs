namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("viewComEmployeeInfo")]
    public partial class viewComEmployeeInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmpId { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? JoinDate { get; set; }

        public int? EmpTypeId { get; set; }

        [StringLength(100)]
        public string EmpType { get; set; }

        [StringLength(20)]
        public string CategoryCode { get; set; }

        public int? DesignationId { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(100)]
        public string Department { get; set; }
    }
}
