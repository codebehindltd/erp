namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PPumpMachineTest")]
    public partial class PPumpMachineTest
    {
        [Key]
        public int TestId { get; set; }

        public DateTime? TestDate { get; set; }

        public int? MachineId { get; set; }

        [StringLength(50)]
        public string BeforeMachineReadNumber { get; set; }

        public decimal? TestQuantity { get; set; }

        [StringLength(50)]
        public string AfterMachineReadNumber { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        [StringLength(20)]
        public string CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [StringLength(20)]
        public string LastModifiedDate { get; set; }
    }
}
