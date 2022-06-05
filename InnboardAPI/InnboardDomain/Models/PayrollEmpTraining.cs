namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTraining")]
    public partial class PayrollEmpTraining
    {
        [Key]
        public int TrainingId { get; set; }

        public int TrainingTypeId { get; set; }

        [StringLength(100)]
        public string Trainer { get; set; }

        public int OrganizerId { get; set; }

        public DateTime? StartDate { get; set; }

        public string AttendeeList { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public bool? Reminder { get; set; }

        public int? ReminderHour { get; set; }

        [StringLength(20)]
        public string Note { get; set; }

        public DateTime? EndDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
