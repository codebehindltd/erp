namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductSerialInfo")]
    public partial class PMProductSerialInfo
    {
        [Key]
        public int SerialId { get; set; }

        public int? ReceivedId { get; set; }

        public int? ReceiveDetailsId { get; set; }

        public int? POrderId { get; set; }

        public int? ItemId { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        public int? SalesId { get; set; }

        [StringLength(25)]
        public string SerialStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
