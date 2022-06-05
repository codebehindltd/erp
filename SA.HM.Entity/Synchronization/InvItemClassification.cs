namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvItemClassification
    {
        public long Id { get; set; }

        [StringLength(300)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        
    }
}
