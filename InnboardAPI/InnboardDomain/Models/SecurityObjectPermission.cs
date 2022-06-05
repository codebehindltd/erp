namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityObjectPermission")]
    public partial class SecurityObjectPermission
    {
        [Key]
        public int ObjectPermissionId { get; set; }

        public int ObjectTabId { get; set; }

        public int UserGroupId { get; set; }

        public bool? IsSavePermission { get; set; }

        public bool? IsUpdatePermission { get; set; }

        public bool? IsDeletePermission { get; set; }

        public bool? IsViewPermission { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
