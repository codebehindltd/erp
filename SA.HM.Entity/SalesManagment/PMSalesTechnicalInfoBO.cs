using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class PMSalesTechnicalInfoBO
    {
        public int TechnicalInfoId { get; set; }
        public int SelectedTechnicalInfoId { get; set; }
        public int CustomerId { get; set; }
        public string TechnicalContactPerson { get; set; }
        public string TechnicalPersonDepartment { get; set; }
        public string TechnicalPersonDesignation { get; set; }
        public string TechnicalPersonPhone { get; set; }
        public string TechnicalPersonEmail { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
