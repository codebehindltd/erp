namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityLogError")]
    public partial class SecurityLogError
    {
        [Key]
        public int ErrorLogId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(500)]
        public string ErrorDetails { get; set; }

        [StringLength(20)]
        public string ErrorStatus { get; set; }
    }
}
