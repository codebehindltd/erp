namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollBestEmployeeNominationDetails
    {
        [Key]
        public long BestEmpNomineeDetailsId { get; set; }

        public long BestEmpNomineeId { get; set; }

        public int EmpId { get; set; }

        public bool? IsSelectedAsMonthlyBestEmployee { get; set; }

        public bool? IsSelectedForYearlyBestEmployee { get; set; }

        public bool? IsSelectedAsYearlyBestEmployee { get; set; }
    }
}
