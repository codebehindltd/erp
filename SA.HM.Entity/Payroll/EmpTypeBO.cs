using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class EmpTypeBO
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TypeCategory { get; set; }
        public string Remarks { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public bool IsServiceChargeApplicable { get; set; }
        public Boolean IsContractualType { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
