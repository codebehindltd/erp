namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountMaster")]
    public partial class DiscountMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiscountMaster()
        {
            DiscountDetail = new HashSet<DiscountDetail>();
        }

        public long Id { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime Todate { get; set; }

        [StringLength(50)]
        public string DiscountFor { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [StringLength(150)]
        public string DiscountName { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int CostCenterId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountDetail> DiscountDetail { get; set; }
    }
}
