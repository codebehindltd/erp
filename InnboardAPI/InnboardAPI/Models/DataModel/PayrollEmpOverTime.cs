namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpOverTime")]
    public partial class PayrollEmpOverTime
    {
        [Key]
        public long OverTimeId { get; set; }

        public int? EmpId { get; set; }

        public DateTime? OverTimeDate { get; set; }

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public int? TotalHour { get; set; }

        public int? OTHour { get; set; }

        public int? ApprovedOTHour { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
