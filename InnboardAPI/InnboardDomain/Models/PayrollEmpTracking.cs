using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Models
{
    public class PayrollEmpTracking
    {
        public int EmpId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime AttDateTime { get; set; }
        public string DeviceInfo { get; set; }
        public string GoogleMapUrl { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
