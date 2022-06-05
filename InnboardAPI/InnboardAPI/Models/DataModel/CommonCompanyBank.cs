namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCompanyBank")]
    public partial class CommonCompanyBank
    {
        [Key]
        public int BankId { get; set; }

        [StringLength(150)]
        public string BankName { get; set; }

        [StringLength(150)]
        public string BranchName { get; set; }

        [StringLength(20)]
        public string SwiftCode { get; set; }

        [StringLength(100)]
        public string AccountName { get; set; }

        [StringLength(20)]
        public string AccountNo1 { get; set; }

        [StringLength(20)]
        public string AccountNo2 { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
