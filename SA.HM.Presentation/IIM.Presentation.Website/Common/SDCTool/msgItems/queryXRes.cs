using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class queryXRes
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
            public string cashier_name { get; set; }
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
            public string session_end_time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string session_start_time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sessionid { get; set; }
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
            public string payType { get; set; }
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
