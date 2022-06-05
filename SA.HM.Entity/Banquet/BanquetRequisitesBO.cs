using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetRequisitesBO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }

        public long CreatedBy { get; set; }
        public long LastModifiedBy { get; set; }
        public long? AccountsPostingHeadId { get; set; }
        public long? ExpenseAccountsPostingHeadId { get; set; }
    }
}
