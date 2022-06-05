using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    public class invoiceReq
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
            public string hsCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string item { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string price { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sd_category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vat_category { get; set; }
        }

        public class dataItems
        {
            /// <summary>
            /// 
            /// </summary>
            public string buyerInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string currency_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<GoodsInfoItem> goodsInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string payType { get; set; }
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

