namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityUserInformation")]
    public partial class SecurityUserInformation
    {
        [Key]
        public int UserInfoId { get; set; }

        public int UserGroupId { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserPassword { get; set; }

        [StringLength(100)]
        public string UserEmail { get; set; }

        [StringLength(50)]
        public string UserPhone { get; set; }

        [StringLength(500)]
        public string UserDesignation { get; set; }

        public bool ActiveStat { get; set; }

        public int? WorkingCostCenterId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? EmpId { get; set; }

        public bool? IsAdminUser { get; set; }
    }
}
