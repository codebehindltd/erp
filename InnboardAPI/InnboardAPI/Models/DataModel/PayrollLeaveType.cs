namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollLeaveType")]
    public partial class PayrollLeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [StringLength(100)]
        public string TypeName { get; set; }

        public int? YearlyLeave { get; set; }

        public int? LeaveModeId { get; set; }

        public bool? CanCarryForward { get; set; }

        public byte? MaxDayCanCarryForwardYearly { get; set; }

        public byte? MaxDayCanKeepAsCarryForwardLeave { get; set; }

        public bool? CanEncash { get; set; }

        public byte? MaxDayCanEncash { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
