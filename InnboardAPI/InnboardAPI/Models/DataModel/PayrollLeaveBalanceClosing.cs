namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollLeaveBalanceClosing")]
    public partial class PayrollLeaveBalanceClosing
    {
        [Key]
        public long LeaveClosingId { get; set; }

        public long EmpId { get; set; }

        public int FiscalYearId { get; set; }

        public int LeaveTypeId { get; set; }

        public decimal OpeningLeave { get; set; }

        public decimal TakenLeave { get; set; }

        public decimal RemainingLeave { get; set; }

        public byte MaxDayCanCarryForwardYearly { get; set; }

        public byte CarryForwardedLeave { get; set; }

        public byte MaxDayCanKeepAsCarryForwardLeave { get; set; }

        public byte TotalCarryforwardLeave { get; set; }

        public byte MaxDayCanEncash { get; set; }

        public byte EncashableLeave { get; set; }

        [StringLength(25)]
        public string ApprovedStatus { get; set; }

        [StringLength(25)]
        public string Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
