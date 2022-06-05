namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpWorkStation")]
    public partial class PayrollEmpWorkStation
    {
        [Key]
        public int WorkStationId { get; set; }

        [StringLength(250)]
        public string WorkStationName { get; set; }
    }
}
