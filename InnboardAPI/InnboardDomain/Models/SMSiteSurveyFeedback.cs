namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSiteSurveyFeedback")]
    public partial class SMSiteSurveyFeedback
    {
        public long Id { get; set; }

        public long? SiteSurveyNoteId { get; set; }

        public string SurveyFeedback { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
