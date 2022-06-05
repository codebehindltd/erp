namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSupplierProductReturn")]
    public partial class PMSupplierProductReturn
    {
        [Key]
        public int ReturnId { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int ReceivedId { get; set; }

        public int? POrderId { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
