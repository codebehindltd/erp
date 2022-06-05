namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLCompany")]
    public partial class GLCompany
    {
        [Key]
        public int CompanyId { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ShortName { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public bool IsProfitableOrganization { get; set; }

        public bool? IsManufacturarOrganization { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
