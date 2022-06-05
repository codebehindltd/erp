namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpLanguage")]
    public partial class PayrollEmpLanguage
    {
        [Key]
        public int LanguageId { get; set; }

        public int EmpId { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(20)]
        public string Reading { get; set; }

        [StringLength(20)]
        public string Writing { get; set; }

        [StringLength(20)]
        public string Speaking { get; set; }
    }
}
