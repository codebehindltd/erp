namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollServiceChargeConfigurationDetails
    {
        [Key]
        public long ServiceChargeConfigurationDetailsId { get; set; }

        public long ServiceChargeConfigurationId { get; set; }

        public int EmpId { get; set; }
    }
}
