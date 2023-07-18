using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models.Membership
{
    public class GetMembershipSetupDataBO
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? SubscriptionFee { get; set; }
        public decimal? MinimumInstallmentSubscriptionFee { get; set; }        
        public decimal? DiscountPercent { get; set; }

        public int CostCenterId { get; set; }
        public string BenefitsType { get; set; }
        public string BenefitsName { get; set; }
        public string BenefitsDetails { get; set; }
        public decimal? BenefitsValue { get; set; }
        public string BenefitsTransactionType { get; set; }

        public string TNCType { get; set; }
        public string TNCName { get; set; }
        public string TNCDetails { get; set; }
        public int DisplaySequence { get; set; }
    }
}
