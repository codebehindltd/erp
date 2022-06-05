using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Maintenance
{
    public class GatePassBO
    {
        public long GatePassId { get; set; }
        public string GatePassNumber { get; set; }
        public DateTime GatePassDate { get; set; }
        public int SupplierId { get; set; }
        public string Remarks { get; set; }
        public int ResponsiblePersonId { get; set; }
        public int? ApprovedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CheckedDate { get; set; }
        public string Status { get; set; }

        //view purpose
        public string Supplier { get; set; }
        public string ResponsiblePerson { get; set; }
        public string ApprovedByPerson { get; set; }
        public string CreatedByPerson { get; set; }
        public string LastModifiedByPerson { get; set; }

    }
}
