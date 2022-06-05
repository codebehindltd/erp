namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollDonor")]
    public partial class PayrollDonor
    {
        [Key]
        public int DonorId { get; set; }

        [StringLength(50)]
        public string DonorCode { get; set; }

        [Required]
        [StringLength(150)]
        public string DonorName { get; set; }
    }
}
