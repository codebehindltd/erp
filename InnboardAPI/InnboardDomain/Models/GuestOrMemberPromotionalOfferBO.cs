using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class GuestOrMemberPromotionalOfferBO
    {        
        public int CostCenterId { get; set; }
        public string BenefitsType { get; set; }
        public string BenefitsName { get; set; }
        public string BenefitsDetails { get; set; }
        public decimal? BenefitsValue { get; set; }
        public decimal? EnjoyedBenefitsValue { get; set; }
        public decimal? RemainingBenefitsValue { get; set; }
        public string BenefitsTransactionType { get; set; }
        public int DisplaySequence { get; set; }
    }
}
