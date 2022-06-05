using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AttendanceDeviceBO
    {
        public string ReaderId { get; set; }
        public string DeviceType { get; set; }
        public string ReaderType { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int DeptIdn { get; set; }
        public string IP { get; set; }
        public string MacAddress { get; set; }
        public int ConnType { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int PortNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
