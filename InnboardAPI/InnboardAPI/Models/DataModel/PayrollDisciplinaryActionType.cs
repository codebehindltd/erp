namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollDisciplinaryActionType")]
    public partial class PayrollDisciplinaryActionType
    {
        [Key]
        public short DisciplinaryActionTypeId { get; set; }

        [Required]
        [StringLength(25)]
        public string ActionName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
