using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeViewBO
    {
        public int EmployeeId { get; set; }
        public int DeviceId { get; set; }
        public int MappingEmployeeId { get; set; }
        public string MappingEmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public String MappingEmployeeName { get; set; }
        public List<EmployeeBO> Employees { get; set; }
        public List<EmployeeBO> MappingEmployees { get; set; }

        public List<Device> Devices = new List<Device>();

        public EmployeeViewBO()
        {
            Devices = new List<Device>();
        }
    }

    public class Device
    {
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
        public List<EmployeeBO> Employees { get; set; }
        public Device()
        {
            Employees = new List<EmployeeBO>();
        }
    }
}
