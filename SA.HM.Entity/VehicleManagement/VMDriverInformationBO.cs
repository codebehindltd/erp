using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.VehicleManagement
{
    public class VMDriverInformationBO
    {
        public long Id { get; set; }
        public string DriverName { get; set; }
        public string DrivingLicenceNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string NID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmergancyContactPerson { get; set; }
        public string EmergancyContactNumber { get; set; }
        public long? EmployeeId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
