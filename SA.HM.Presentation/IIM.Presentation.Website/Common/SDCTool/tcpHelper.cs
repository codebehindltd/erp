using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InnBoardSDC.SDCTool
{
    class tcpHelper
    {
        public Socket tcpClientObj;//Send and receive data

        public delegate bool receiveDelegate(byte[] receiveData);//The type of method to handle receiving data events
        //public event receiveDelegate receiveEvent; //Data receiving event

        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        byte[] m_ReceiverByte = new byte[204800];
        public delegate int SomeFunWay(int iRet, string strRt);
        SomeFunWay m_callFun;

        public bool SendConnection(string ip, int port, SomeFunWay CBFun)  //Send connection by IP address and port number
        {
            m_callFun = CBFun;
            IPAddress ipaddr = IPAddress.Parse(ip);//After converting to an IP address, the connection will speed up

            tcpClientObj = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Connect client
            bool bt = false;

            do
            {
                try
                {
                    tcpClientObj.Connect(ipaddr, port);//Connection

                }
                catch (Exception ex)
                {
                    //bt = true;
                    m_callFun(-1, ex.Message);
                }


            } while (bt);
            try
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new System.EventHandler<SocketAsyncEventArgs>(MyCompleted);

                args.SetBuffer(m_ReceiverByte, 0, m_ReceiverByte.Length);
                tcpClientObj.ReceiveAsync(args);


            }
            catch (Exception ex)
            {
                m_callFun(-1, ex.Message);
                return false;
            }
            return true;
        }

        private void MyCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket socket = sender as Socket;
            IPEndPoint client = (IPEndPoint)socket.RemoteEndPoint;//Get client ip
            string clientIp = client.Address.ToString();
            try
            {
                if (e.SocketError == System.Net.Sockets.SocketError.Success && e.BytesTransferred > 0)
                {

                    if (e.Buffer.Length > 0)
                    {
                        Byte[] team = new Byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, team, e.BytesTransferred);
                        m_callFun(0, System.Text.Encoding.Default.GetString(team));
                    }
                }
                else
                {

                }
            }
            catch (Exception e1)
            {
                LogHelper.Save("MyCompleted", e1.Message);
            }
        }

        public void Send(byte[] by, int length) //send Message
        {
            if (tcpClientObj == null)
            {
                return;
            }
            if (tcpClientObj.Connected)
            {
                tcpClientObj.Send(by, length, SocketFlags.None);// Send, the length is the length of the data in the memory stream
            }
        }

        public void closeClient()
        {
            if (tcpClientObj.Connected)
            {
                tcpClientObj.Shutdown(SocketShutdown.Both);
                tcpClientObj.Close();
            }
        }
    }

    public class paramTCP
    {
        public string msg;
        public string ip;
        public int port;
    }
}
