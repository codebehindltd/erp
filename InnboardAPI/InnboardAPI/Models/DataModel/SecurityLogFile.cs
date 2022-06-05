namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityLogFile")]
    public partial class SecurityLogFile
    {
        [Key]
        [Column(Order = 0)]
        public int LogId { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime LogDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(25)]
        public string InpuTMode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(900)]
        public string LogMemo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string LogMode { get; set; }
    }
}
