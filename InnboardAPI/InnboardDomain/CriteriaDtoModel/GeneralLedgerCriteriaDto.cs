using InnboardDomain.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.CriteriaDtoModel
{
    public class GeneralLedgerCriteriaDto
    {
        public GeneralLedgerCriteriaDto()
        {
            pageParams = new PageParams();
        }
        public PageParams pageParams { get; set; }
        public int? companyId { get; set; }
        public int? projectId { get; set; }
        public int userInfoId { get; set; }
        public int userGroupId { get; set; }
         public string voucherType { get; set; }
        public string voucherStatus { get; set; }
        public string voucherNo { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string referenceNo { get; set; }
        public string referenceVoucherNo { get; set; }
        public string narration { get; set; }
        //public int recordPerPage { get; set; }
        //public int pageIndex { get; set; } out int totalRecords
    }
}
