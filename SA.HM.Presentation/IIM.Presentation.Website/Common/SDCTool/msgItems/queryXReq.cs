using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class queryXReq
    {
        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public string startDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string endDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string limit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string offset { get; set; }
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
        public string data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }
}
