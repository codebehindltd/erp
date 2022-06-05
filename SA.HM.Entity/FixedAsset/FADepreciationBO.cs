using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.FixedAsset
{
    public class FADepreciationBO
    {
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int FiscalYearId { get; set; }
        public int AccountHeadId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
