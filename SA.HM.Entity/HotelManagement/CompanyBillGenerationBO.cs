using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyBillGenerationBO
    {
        public long CompanyBillId { get; set; }
        public int CompanyId { get; set; }
        public int BillCurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTime BillDate { get; set; }
        public string CompanyBillNumber { get; set; }
        public string Remarks { get; set; }
        public string ApprovedStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public string CompanyName { get; set; }
    }
}
