namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GLAccountTypeSetup")]
    public partial class GLAccountTypeSetup
    {
        [Key]
        public int AccountTypeId { get; set; }

        public int? NodeId { get; set; }

        [StringLength(5)]
        public string AccountType { get; set; }
    }
}
