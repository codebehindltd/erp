using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.UserInformation
{
    public class UserConfigurationBO
    {
        public long Id { get; set; }
        public long FeaturesId { get; set; }
        public long UserInfoId { get; set; }
        public bool IsCheckedBy { get; set; }
        public bool IsApprovedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
