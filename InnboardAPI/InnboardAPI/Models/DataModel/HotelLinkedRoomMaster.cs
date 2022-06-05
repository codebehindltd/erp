namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelLinkedRoomMaster")]
    public partial class HotelLinkedRoomMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelLinkedRoomMaster()
        {
            HotelLinkedRoomDetails = new HashSet<HotelLinkedRoomDetails>();
        }

        public long Id { get; set; }

        public long RegistrationId { get; set; }

        [StringLength(50)]
        public string LinkName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelLinkedRoomDetails> HotelLinkedRoomDetails { get; set; }
    }
}
