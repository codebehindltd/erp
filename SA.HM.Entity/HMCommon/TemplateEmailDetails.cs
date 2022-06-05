using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class TemplateEmailDetails
    {
        public long Id { get; set; }
        public long? TemplateEmailId { get; set; }
        public long? EmployeeId { get; set; }
    }
}
