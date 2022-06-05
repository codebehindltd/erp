using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMStage
    {
        public int Id { get; set; }
        public string StageName { get; set; }
        public string StageType { get; set; }
        public int? DisplaySequence { get; set; }
        public bool IsActive { get; set; }        
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsCompanyStageChange { get; set; }
        public bool IsQuotationSentStage { get; set; }
        public bool IsEditNDeleteDisable { get; set; }
        
    }
}
