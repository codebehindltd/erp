namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountConfigSetup")]
    public partial class DiscountConfigSetup
    {
        [Key]
        public long ConfigurationId { get; set; }

        public bool? IsDiscountApplicableIndividually { get; set; }

        public bool? IsDiscountApplicableMaxOneWhenMultiple { get; set; }

        public bool? IsDiscountOptionShowsWhenMultiple { get; set; }

        public bool? IsDiscountAndMembershipDiscountApplicableTogether { get; set; }

        public bool? IsBankDiscount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
