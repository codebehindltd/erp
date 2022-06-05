using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class DiscountMasterBO
    {
        public DiscountMasterBO()
        {
            this.DiscountDetails = new List<DiscountDetailBO>();
        }

        public long Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        public string DiscountFor { get; set; }
        public string Remarks { get; set; }
        public string DiscountName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int CostCenterId { get; set; }

        public string CostCenter { get; set; }

        public virtual List<DiscountDetailBO> DiscountDetails { get; set; }
    }
}
