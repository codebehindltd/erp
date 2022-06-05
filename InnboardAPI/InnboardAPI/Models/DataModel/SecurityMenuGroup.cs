namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityMenuGroup")]
    public partial class SecurityMenuGroup
    {
        [Key]
        public long MenuGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuGroupName { get; set; }

        [StringLength(50)]
        public string GroupDisplayCaption { get; set; }

        public int DisplaySequence { get; set; }

        [StringLength(25)]
        public string GroupIconClass { get; set; }

        public bool ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
