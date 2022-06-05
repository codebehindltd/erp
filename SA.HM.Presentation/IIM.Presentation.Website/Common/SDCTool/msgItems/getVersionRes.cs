using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class getVersionRes
    {
        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public string currency_version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rateCalculationMethod { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sditem_version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string taxItemVersion { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vatitem_version { get; set; }
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
