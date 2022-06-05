namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonMessage")]
    public partial class CommonMessage
    {
        [Key]
        public long MessageId { get; set; }

        public int MessageFrom { get; set; }

        public DateTime MessageDate { get; set; }

        [StringLength(25)]
        public string Importance { get; set; }

        [StringLength(350)]
        public string Subjects { get; set; }

        [StringLength(1000)]
        public string MessageBody { get; set; }
    }
}
