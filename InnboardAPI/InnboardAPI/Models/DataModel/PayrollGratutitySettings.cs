namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollGratutitySettings
    {
        [Key]
        public int GratuityId { get; set; }

        public int GratuityWillAffectAfterJobLengthInYear { get; set; }

        public bool? IsGratuityBasedOnBasic { get; set; }

        public bool? IsGratutityBasedOnGross { get; set; }

        public decimal? GratutiyPercentage { get; set; }

        public int NumberOfGratuityAdded { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
