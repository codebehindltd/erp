using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class DynamicReportBO
    {
        public int ColumnId { get; set; }
        public int RowId { get; set; }
        public string ColumnName { get; set; }
        public object ColName { get; set; }
        public object ItemValue { get; set; }
        public string DataType { get; set; }
    }
}
