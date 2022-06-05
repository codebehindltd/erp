namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BanquetInformation")]
    public partial class BanquetInformation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BanquetInformation()
        {
            BanquetReservation = new HashSet<BanquetReservation>();
        }

        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public decimal? Capacity { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? CostCenterId { get; set; }

        public long? AccountsPostingHeadId { get; set; }

        public int? Status { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public virtual GLNodeMatrix GLNodeMatrix { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetReservation> BanquetReservation { get; set; }
    }
}
