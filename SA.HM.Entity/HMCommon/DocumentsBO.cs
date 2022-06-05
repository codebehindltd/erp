using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
   public class DocumentsBO
    {
        public int Id { get; set; }
        public int CostCenterId { get;  set;}
        public long? DocumentId { get; set; }
        public long? OwnerId { get; set; }
        public string DocumentCategory { get; set; }
        public string DocumentType { get; set; }
        public string Extention { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByDate { get; set; }
        public int LastModified { get; set; }
        public string LastModifiedDate { get; set; }
        public string IconImage { get; set;}
        public string Instruction { get; set; }
        public string ImageUrl { get; set; }
        public string VacantImagePath { get; set; }
        public string OccupiedImagePath { get; set; }
        public string EmployeeName { get; set; }
    }
}
