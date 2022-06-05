using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesOrderBO
    {
        public int SOrderId { get; set; }
        public int BillId { get; set; }
        public DateTime SODate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string SONumber { get; set; }
        public int CompanyId { get; set; }        
        public string ApprovedStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public string Remarks { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }        
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string CompanyName { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public decimal MonthTarget { get; set; }
        public decimal MonthAchievement { get; set; }
    }
}
