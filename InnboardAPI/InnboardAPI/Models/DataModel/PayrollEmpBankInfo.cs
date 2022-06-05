namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpBankInfo")]
    public partial class PayrollEmpBankInfo
    {
        [Key]
        [Column(Order = 0)]
        public int BankInfoId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmpId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BankId { get; set; }

        [StringLength(250)]
        public string BranchName { get; set; }

        [StringLength(250)]
        public string AccountName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(25)]
        public string AccountType { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }
    }
}
