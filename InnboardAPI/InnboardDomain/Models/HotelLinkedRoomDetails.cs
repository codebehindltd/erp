namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HotelLinkedRoomDetails
    {
        public long Id { get; set; }

        public long MasterId { get; set; }

        public long RegistrationId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public virtual HotelLinkedRoomMaster HotelLinkedRoomMaster { get; set; }
    }
}
