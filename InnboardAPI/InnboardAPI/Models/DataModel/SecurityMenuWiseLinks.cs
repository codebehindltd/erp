namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SecurityMenuWiseLinks
    {
        [Key]
        public long MenuWiseLinksId { get; set; }

        public int UserGroupId { get; set; }

        public long MenuGroupId { get; set; }

        public long MenuLinksId { get; set; }

        public int DisplaySequence { get; set; }

        public bool IsSavePermission { get; set; }

        public bool? IsUpdatePermission { get; set; }

        public bool IsDeletePermission { get; set; }

        public bool IsViewPermission { get; set; }

        public bool ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
