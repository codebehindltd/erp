namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMDealWiseContactMap")]
    public partial class SMDealWiseContactMap
    {
        public long Id { get; set; }

        public long? DealId { get; set; }

        public long? ContactId { get; set; }
    }
}
