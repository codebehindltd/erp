namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityUserGroup")]
    public partial class SecurityUserGroup
    {
        [Key]
        public int UserGroupId { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        public int? DefaultHomePageId { get; set; }

        public bool? IsGroupApplicable { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
