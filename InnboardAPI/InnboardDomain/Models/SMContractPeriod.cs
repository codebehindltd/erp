namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMContractPeriod")]
    public partial class SMContractPeriod
    {
        [Key]
        public int ContractPeriodId { get; set; }

        [Required]
        [StringLength(250)]
        public string ContractPeriodName { get; set; }

        public short ContractPeriodValue { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
