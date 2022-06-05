using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class creditNoteReq
    {

        public class GoodsInfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qty { get; set; }
        }

        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public List<GoodsInfoItem> goodsInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string invoiceNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mobile { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string taskID { get; set; }
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
