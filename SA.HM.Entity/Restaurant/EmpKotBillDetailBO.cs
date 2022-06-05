using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class EmpKotBillDetailBO
    {
        public int DetailId { get; set; }
        public int EmpId { get; set; }
        public string BillNumber { get; set; }
        public int KotId { get; set; }
        public int KotDetailId { get; set; }
        public string KotDetailIdList { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string ItemName { get; set; }
        public DateTime JobStartDate { get; set; }
        public string JobStartDateString { get; set; }
        public DateTime JobEndDate { get; set; }
        public string JobEndDateString { get; set; }
        public string JobStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateString { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
