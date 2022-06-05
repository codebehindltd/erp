namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLNodeMatrix")]
    public partial class GLNodeMatrix
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GLNodeMatrix()
        {
            BanquetInformation = new HashSet<BanquetInformation>();
            BanquetRequisites = new HashSet<BanquetRequisites>();
            BanquetReservationBillPayment = new HashSet<BanquetReservationBillPayment>();
            HotelRoomType = new HashSet<HotelRoomType>();
        }

        [Key]
        public long NodeId { get; set; }

        public int? AncestorId { get; set; }

        [Required]
        [StringLength(50)]
        public string NodeNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string NodeHead { get; set; }

        public int Lvl { get; set; }

        [StringLength(900)]
        public string Hierarchy { get; set; }

        [StringLength(900)]
        public string HierarchyIndex { get; set; }

        public bool NodeMode { get; set; }

        [StringLength(25)]
        public string NodeType { get; set; }

        [StringLength(50)]
        public string NotesNumber { get; set; }

        public bool? IsTransactionalHead { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetInformation> BanquetInformation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetRequisites> BanquetRequisites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BanquetReservationBillPayment> BanquetReservationBillPayment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelRoomType> HotelRoomType { get; set; }
    }
}
