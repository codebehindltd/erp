namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMTask")]
    public partial class SMTask
    {
        public long Id { get; set; }

        [StringLength(150)]
        public string TaskName { get; set; }

        [Column(TypeName = "date")]
        public DateTime DueDate { get; set; }

        public TimeSpan DueTime { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string TaskType { get; set; }

        public int? AssignToId { get; set; }

        [StringLength(50)]
        public string EmailReminderType { get; set; }

        public bool IsEmailReminderSent { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmailReminderDate { get; set; }

        public TimeSpan? EmailReminderTime { get; set; }

        public bool IsCompleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public virtual SMAccountManager SMAccountManager { get; set; }
    }
}
