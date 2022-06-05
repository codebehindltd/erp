namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMSiteSurveyFeedbackDetails
    {
        public long Id { get; set; }

        public long SiteSurveyFeedbackId { get; set; }

        public int ItemId { get; set; }

        public decimal Quantity { get; set; }
    }
}
