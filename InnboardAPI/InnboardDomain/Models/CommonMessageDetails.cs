namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonMessageDetails
    {
        [Key]
        public long MessageDetailsId { get; set; }

        public long MessageId { get; set; }

        public int MessageTo { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public bool? IsReaden { get; set; }
    }
}
