using HotelManagement.Entity.UserInformation;
//using HotelManagement.Presentation.Website.Common;
using sdcTool.msgItems;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InnBoardSDC.SDCTool
{
    public class InvoiceHandler
    {
        private paramTCP m_tcpParam = new paramTCP();
        //private const int TCP_CONNECT_ERROR = 200 + 1;
        //private const int TCP_CATCH_ERROR = 200 + 2;
        private JsonHelper m_jsonHelp = new JsonHelper();
        private udpHelp m_udpHelp = new udpHelp();
        private CryptHelper m_cryptHelp = new CryptHelper();
        private tcpHelper m_tcpHelp = new tcpHelper();
        
        private byte[] m_keyByte;
        
        private sdcTool.msgItems.invoiceReq param = new sdcTool.msgItems.invoiceReq();
        private sdcTool.msgItems.invoiceReq.dataItems dataItem = new sdcTool.msgItems.invoiceReq.dataItems();

        public delegate void InvoiceDataReceive(UserInformationBO userInformationBO, int BillId, invoiceRes invoiceResponse);
        public event InvoiceDataReceive OnInvoiceDataReceive;

        public delegate void DeviceConnectionFailed(string message);
        public event DeviceConnectionFailed OnDeviceConnectionFailed;

        public string CashierId { get
            {
                return param.cashierID;
            }
            set
            {
                param.cashierID = value;
            }
        }

        public string CheckCode
        {
            get
            {
                return param.checkCode;
            }
            set
            {
                param.checkCode = value;
            }
        }

        public string InvoiceType
        {
            get
            {
                return param.type;
            }
            set
            {
                param.type = value;
            }
        }

        public int BillId { get; set; }

        public UserInformationBO UInfoBO { get; set; }

        public string BuyerInfo { get; set; }

        public string CurrencyCode { get; set; }
        public string PayType { get; set; }

        public InvoiceHandler()
        {
            //m_tcpParam.ip = GetConfig("ip");
            m_tcpParam.ip = System.Web.Configuration.WebConfigurationManager.AppSettings["ip"];
            //m_tcpParam.port = int.Parse(GetConfig("port_tcp"));
            m_tcpParam.port = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["port_tcp"]);

            //param.cashierID = "11";
            //param.checkCode = "5AEAD1D3D154FE1D39836A37023663DF9BE0DFEF9398EBD4141AA0A0A9DA07F3";

            //dataItem.buyerInfo = "1123132";
            //dataItem.buyerInfo = this.BuyerInfo;
            //dataItem.currency_code = "BDT";
            //dataItem.payType = "PAYTYPE_CASH";
            dataItem.taskID = Guid.NewGuid().ToString("N");
        }

        public void DoInvoice(List<invoiceReq.GoodsInfoItem> goodsInfo)
        {
            dataItem.goodsInfo = goodsInfo;
            this.ProcessInvoice();
        }

        public void DoInvoice(invoiceReq.GoodsInfoItem goodItem)
        {
            List<sdcTool.msgItems.invoiceReq.GoodsInfoItem> goodsInfo = new List<sdcTool.msgItems.invoiceReq.GoodsInfoItem>();
            goodsInfo.Add(goodItem);
            dataItem.goodsInfo = goodsInfo;
            this.ProcessInvoice();
        }

        public void DoInvoice(string item_name, string code, string sd, string vat, string price, string qty)
        {
            List<sdcTool.msgItems.invoiceReq.GoodsInfoItem> goodsInfo = new List<sdcTool.msgItems.invoiceReq.GoodsInfoItem>();
            sdcTool.msgItems.invoiceReq.GoodsInfoItem goodItem = new sdcTool.msgItems.invoiceReq.GoodsInfoItem();
            goodItem.code = code;// "A0001";
            goodItem.hsCode = "";
            goodItem.item = item_name;// "apple";
            goodItem.price = price;
            goodItem.qty = qty;
            goodItem.sd_category = sd;// "13701";
            goodItem.vat_category = vat; // "13352";
            goodsInfo.Add(goodItem);
            dataItem.goodsInfo = goodsInfo;

            this.ProcessInvoice();
        }

        private void ProcessInvoice()
        {
            dataItem.buyerInfo = this.BuyerInfo;
            dataItem.currency_code = this.CurrencyCode;
            dataItem.payType = this.PayType;

            param.data = JsonHelper.SerializeObjct(dataItem);
            param.type = this.InvoiceType;
            //param.type = "SDCA0000";
            
            param.checkCode = m_cryptHelp.sha256(param.data).ToLower();

            Thread workthread = new Thread(new ParameterizedThreadStart(invoiceThread));
            workthread.Start(param);
        }

        private void invoiceThread(object obj)
        {
            sdcTool.msgItems.invoiceReq param = (sdcTool.msgItems.invoiceReq)obj;
            string strSrc = JsonHelper.SerializeObjct(param);
            byte[] data = packSendData(strSrc);

            if (!m_tcpHelp.SendConnection(m_tcpParam.ip, m_tcpParam.port, invoiceThreadRespon))
            {
                string msg = "connect tcp server fail. ip = " + m_tcpParam.ip + " port = " + m_tcpParam.port;
                //showLogUI(msg);
                return;
            }
            m_tcpHelp.Send(data, data.Length);
        }

        private int invoiceThreadRespon(int iRet, string strRt)
        {
            if(iRet != -1)
            {
                threadStatus rt = new threadStatus();
                rt.status = iRet;
                rt.message = strRt;

                string strMsg = parseRecvData(rt.message);
                sdcTool.msgItems.invoiceRes resMsg = (sdcTool.msgItems.invoiceRes)JsonHelper.JsonConvertObject<sdcTool.msgItems.invoiceRes>(strMsg);

                if (OnInvoiceDataReceive != null)
                {
                    OnInvoiceDataReceive(UInfoBO, this.BillId, resMsg);
                }
                return 0;
            }
            else
            {
                if(OnDeviceConnectionFailed != null)
                {
                    OnDeviceConnectionFailed(strRt);
                }
                return iRet;
            }
        }

        private string parseRecvData(string strRt)
        {
            string strKey = getKey("123456");
            m_keyByte = m_cryptHelp.HexStringToByteArray(strKey);
            string sData = strRt.Substring(6, strRt.Length - 6);
            string strMsg = m_cryptHelp.AesDecrypt(sData, m_keyByte);
            return strMsg;
        }

        private byte[] packSendData(string strSrc)
        {
            string strKey = getKey("123456");
            
            m_keyByte = m_cryptHelp.HexStringToByteArray(strKey);
            string strRet = m_cryptHelp.AesEncrypt(strSrc, m_keyByte);
            
            string sSend = string.Format("{0:D6}", strRet.Length) + strRet; //string.format("%06d%s", msg.getBytes().length, msg);
            //byte[] data = Encoding.UTF8.GetBytes(sSend);
            return Encoding.UTF8.GetBytes(sSend);
        }

        private string getKey(string data)
        {
            string strMD5 = m_cryptHelp.GenerateMD5(data).ToUpper();
            string strSHA256 = m_cryptHelp.sha256(strMD5);
            return strSHA256;
        }

        private static string GetConfig(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return null;
        }
    }
}
