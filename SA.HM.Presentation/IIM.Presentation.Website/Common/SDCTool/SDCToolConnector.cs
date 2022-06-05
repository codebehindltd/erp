using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using static InnBoardSDC.SDCTool.udpHelp;
using System.Threading;

namespace InnBoardSDC.SDCTool
{
    public class SDCToolConnector
    {
        private byte[] m_keyByte;
        private CryptHelper m_cryptHelp = new CryptHelper();
        private udpHelp m_udpHelp = new udpHelp();
        Dictionary<string, string> devMap = new Dictionary<string, string>();

        public delegate void SdcToolConnection(Dictionary<string, string> keyValuePairs);
        public event SdcToolConnection OnSdcToolConnected;

        public SDCToolConnector()
        {
            string strKey = getKey(this.GetConfig("szzt"));
            
            m_keyByte = m_cryptHelp.HexStringToByteArray(strKey);
        }

        public void ConnectToDevice()
        {
            string strSrc = "SZZT|" + this.GetConfig("serial_number");
            string strRet = m_cryptHelp.AesEncrypt(strSrc, m_keyByte);
            findDevice(strRet, int.Parse(this.GetConfig("port_udp")));
        }

        private void findDevice(string message, int port)
        {
            paramSend param = new paramSend();
            param.port = port;
            param.msg = message;
            Thread workthread = new Thread(new ParameterizedThreadStart(sendMsgUDP));
            workthread.Start(param);
        }

        private void sendMsgUDP(object obj)
        {
            paramSend param = (paramSend)obj;
            
            m_udpHelp.sendMsg(param.msg, param.port, recvFunUDP);
        }

        public int recvFunUDP(int iRet, string strRt)
        {
            if (0 == iRet)
            {
                
                string strMsg = m_cryptHelp.AesDecrypt(strRt, m_keyByte);
                
                string[] sArray = strMsg.Split('|');
                addDev(sArray[3] + "_" + sArray[2], sArray[1]);
                // setEnabelUI(true);
            }
            else
            {
                
            }
            return 0;
        }

        private void addDev(string strKey, string data)
        {
            if (!devMap.ContainsKey(strKey))
            {
                devMap.Add(strKey, data);
                if(OnSdcToolConnected != null)
                {
                    OnSdcToolConnected(devMap);
                }
            }
        }

        private string getKey(string data)
        {
            string strMD5 = m_cryptHelp.GenerateMD5(data).ToUpper();
            string strSHA256 = m_cryptHelp.sha256(strMD5);
            return strSHA256;
        }

        private string GetConfig(string key)
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
