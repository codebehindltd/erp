namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAttendanceDevice")]
    public partial class PayrollAttendanceDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReaderId { get; set; }

        [StringLength(50)]
        public string ReaderType { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public int Type { get; set; }

        public int DeptIdn { get; set; }

        [Required]
        [StringLength(32)]
        public string IP { get; set; }

        [Required]
        [StringLength(32)]
        public string MacAddress { get; set; }

        public int ConnType { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
