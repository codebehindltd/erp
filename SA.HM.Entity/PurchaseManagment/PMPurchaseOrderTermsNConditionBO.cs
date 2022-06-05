using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMPurchaseOrderTermsNConditionBO
    {
        public long Id { get; set; }
        public long TermsNConditionsId { get; set; }
        public int PurchaseId { get; set; }
        public string Title { get; set; }
        public int? DisplaySequence { get; set; }
        public string Description { get; set; }
    }
}
