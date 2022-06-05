using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class TemplateInformationDetailBO
    {
        public long Id { get; set; }
        public Nullable<long> TemplateId { get; set; }
        public string Template { get; set; }
        public string BodyText { get; set; }
        public string ReplacedBy { get; set; }
        public string TableName { get; set; }
    }
}
