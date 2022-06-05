using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMDealStage
    {
        public int Id { get; set; }
        public string DealStage { get; set; }
        public Nullable<decimal>Complete { get; set; }
        public string ForcastType { get; set; }
        public string ForcastCategory { get; set; }
        public Nullable<int> DisplaySequence { get; set; }
        public string Description { get; set; }
        public bool IsSiteSurvey { get; set; }
        public bool IsQuotationReveiw { get; set; }
        public bool IsCloseWon { get; set; }
        public bool IsCloseLost { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

    }
}
