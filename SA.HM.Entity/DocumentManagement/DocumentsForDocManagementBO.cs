using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.DocumentManagement
{
    public class DocumentsForDocManagementBO
    {
        public long DocumentId { get; set; }
        public long? OwnerId { get; set; }
        public string DocumentCategory { get; set; }
        public string DocumentType { get; set; }
        public string Extention { get; set; }
        public string Name { get; set; }
        public string DocumentName { get; set; }
        public string Path { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedByDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string IconImage { get; set; }
    }
}
