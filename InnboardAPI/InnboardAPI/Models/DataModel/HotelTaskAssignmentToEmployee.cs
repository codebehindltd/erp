namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelTaskAssignmentToEmployee")]
    public partial class HotelTaskAssignmentToEmployee
    {
        [Key]
        public long EmpTaskId { get; set; }

        public long TaskId { get; set; }

        public int EmpId { get; set; }
    }
}
