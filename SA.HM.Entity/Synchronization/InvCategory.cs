namespace HotelManagement.Entity.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public int? AncestorId { get; set; }

        public int Lvl { get; set; }

        [StringLength(900)]
        public string Hierarchy { get; set; }

        [StringLength(900)]
        public string HierarchyIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ServiceType { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public bool? ShowInInvoice { get; set; }

        public bool ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
