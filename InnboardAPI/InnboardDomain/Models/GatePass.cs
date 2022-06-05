namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GatePass")]
    public partial class GatePass
    {
        public long GatePassId { get; set; }

        [StringLength(20)]
        public string GatePassNumber { get; set; }

        public DateTime GatePassDate { get; set; }

        public int SupplierId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? ResponsiblePersonId { get; set; }

        public int? ApprovedBy { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? CheckedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? CheckedDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
