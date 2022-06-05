namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSiteSurveyEngineer")]
    public partial class SMSiteSurveyEngineer
    {
        public long Id { get; set; }

        public long? SiteSurveyFeedbackId { get; set; }

        public int? EmpId { get; set; }
    }
}
