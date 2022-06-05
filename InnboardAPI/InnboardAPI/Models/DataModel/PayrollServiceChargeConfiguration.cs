namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollServiceChargeConfiguration")]
    public partial class PayrollServiceChargeConfiguration
    {
        [Key]
        public long ServiceChargeConfigurationId { get; set; }

        public decimal? ServiceAmount { get; set; }

        public short? TotalEmployee { get; set; }

        public int? DepartmentId { get; set; }

        public int? EmpTypeId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
