namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemMemberFamilyMember")]
    public partial class MemMemberFamilyMember
    {
        public int Id { get; set; }

        public int? MemberId { get; set; }

        [StringLength(200)]
        public string MemberName { get; set; }

        public DateTime? MemberDOB { get; set; }

        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        [StringLength(20)]
        public string UsageMode { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
