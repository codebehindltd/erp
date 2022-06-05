namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMAccountManager")]
    public partial class SMAccountManager
    {
        [Key]
        public int AccountManagerId { get; set; }

        public int DepartmentId { get; set; }

        public int EmpId { get; set; }

        [StringLength(50)]
        public string SortName { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
