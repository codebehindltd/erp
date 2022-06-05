using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Maintenance
{
    public class SupplierNCompanyInfoForGatePassInvoiceBO
    {
        public Int64 GatePassId { get; set; }
        public string GatePassNumber { get; set; }
        public DateTime GatePassDate { get; set; }
        public int SupplierId { get; set; }
        public string Remarks { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public String Description { get; set; }
        public string HeadName { get; set; }

        public string CreatedByName { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
    }
}
