using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    class creditNoteRes
    {
        public class GoodsInfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string item { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string product_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rateType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sdAmt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sd_category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sd_value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unitPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vatAmt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vat_category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vat_value { get; set; }
        }

        public class dataItems
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
            public string client_invoice_datetime { get; set; }
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
            public string sdAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string seller_addr { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string seller_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string taxAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string terminal { get; set; }
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
