using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class MachineTestBO
    {
        public int TestId { get; set; }
        public DateTime TestDate { get; set; }
        public string TestDateString { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public string BeforeMachineReadNumber { get; set; }
        public decimal TestQuantity { get; set; }
        public string AfterMachineReadNumber { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string UserName { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
