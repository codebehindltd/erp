namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonCountries
    {
        [Key]
        public int CountryId { get; set; }

        [StringLength(50)]
        public string CountryName { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(2)]
        public string Code2Digit { get; set; }

        [StringLength(3)]
        public string Code3Digit { get; set; }

        [StringLength(3)]
        public string CodeNumeric { get; set; }

        [StringLength(50)]
        public string SBCode { get; set; }
    }
}
