namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvCogsAccountVsItemCategoryMappping
    {
        [Key]
        public int CogsAccountMapId { get; set; }

        public int NodeId { get; set; }

        public int CategoryId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
