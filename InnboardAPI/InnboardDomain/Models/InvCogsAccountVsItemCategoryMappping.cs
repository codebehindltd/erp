namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvCogsAccountVsItemCategoryMappping")]
    public partial class InvCogsAccountVsItemCategoryMappping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int CogsAccountMapId { get; set; }

        public int NodeId { get; set; }

        public int CategoryId { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
