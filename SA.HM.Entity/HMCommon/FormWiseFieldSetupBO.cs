using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class FormWiseFieldSetupBO
    {

        public long Id { get; set; } 
        public int PageId { get; set; }
        public string PageIdStr { get; set; }
        public string FieldId { get; set; }
        public string PageName { get; set; }
        public string FieldName { get; set; }
        public bool IsMandatory { get; set; }
        public bool? IsSaveActivity { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
