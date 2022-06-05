using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdcTool.msgItems
{
    public class invoiceRes
    {
        public class GoodsInfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string itemCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string item_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string noTaxAmt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string oriPrice { get; set; }
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
            public string supplementary_duty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string taxAmt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tax_due { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit_price { get; set; }
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
            public string approvalCode { get; set; }
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
            public string invoiceCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string invoiceNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string payType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qrCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rateDetAmt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rateDetV { get; set; }
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
            /// <summary>
            /// 
            /// </summary>
            public string totalAmount { get; set; }
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
