using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class getTaxRateRes
    {
        public class RatesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string slNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vatRate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string serviceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string serviceCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sdType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string effectFrom { get; set; }
        }
        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public string payType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RatesItem> rates { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vat_item_version { get; set; }
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
