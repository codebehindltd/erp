using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class BaseEntity
    {
        public virtual int? CreatedBy { get; set; }
        public virtual int? LastModifiedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? LastModifiedDate { get; set; }
        
    }
}
