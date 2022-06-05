namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCheckedByApprovedBy")]
    public partial class CommonCheckedByApprovedBy
    {
        public long Id { get; set; }

        public long? FeaturesId { get; set; }

        public long? UserInfoId { get; set; }

        public bool? IsCheckedBy { get; set; }

        public bool? IsApprovedBy { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
