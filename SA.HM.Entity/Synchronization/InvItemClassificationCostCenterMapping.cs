namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvItemClassificationCostCenterMapping
    {
        [Key]
        public long MappingId { get; set; }

        public long? CostCenterId { get; set; }

        public long? ClassificationId { get; set; }

        public long? AccountsPostingHeadId { get; set; }
        
    }
}
