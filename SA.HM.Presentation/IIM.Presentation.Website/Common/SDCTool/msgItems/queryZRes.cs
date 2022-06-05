using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class queryZRes
    {
        public class ListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bin { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cardNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cashNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string createTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string creditNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string isPrint { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string isUp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string report_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string report_date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string report_drawer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sdAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string seller_address { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string seller_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string session_qty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sn { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tax_amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string total_invoice_number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string void_amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string void_sdAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string void_tax_amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string void_totalAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string zDetail { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string zVoidDetail { get; set; }
        }

        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ListItem> list { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string total { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string cashierID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }
}
