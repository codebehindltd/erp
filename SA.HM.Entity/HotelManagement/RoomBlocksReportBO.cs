using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomBlocksReportBO
    {
        public int? RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Reference { get; set; }
        public int? RoomTypeId { get; set; }
        public string TypeCode { get; set; }
        public string RoomType { get; set; }
        public string StatusName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}
