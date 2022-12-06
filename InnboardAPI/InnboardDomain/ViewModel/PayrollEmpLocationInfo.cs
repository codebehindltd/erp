using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class PayrollEmpLocationInfo
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string PresentPhone { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? TrackingAttDateTime { get; set; }
        public DateTime? TrackingCreatedDate { get; set; }
        public string DeviceInfo { get; set; }
        public string GoogleMapUrl { get; set; }
    }
}
