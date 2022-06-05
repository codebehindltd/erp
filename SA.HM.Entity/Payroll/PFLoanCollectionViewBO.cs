using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFLoanCollectionViewBO
    {
        public DateTime CollectionDate { get; set; }
        public decimal? Advance { get; set; }
        public decimal? Refund { get; set; }
        public decimal? Interest { get; set; }
        public decimal? Balance { get; set; }
    }
}
